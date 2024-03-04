using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Helpers;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

using static SDRGames.Whist.DialogueSystem.Models.DialogueAnswerCondition;
using static SDRGames.Whist.DialogueSystem.ScriptableObjects.DialogueScriptableObject;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public class BaseNode : Node
    {
        public string ID { get; set; }
        public string DialogueName { get; set; }
        public DialogueTypes DialogueType { get; set; }
        public Group Group { get; set; }
        public List<AnswerSaveData> Answers { get; set; }

        protected GraphView graphView;
        private Color defaultBackgroundColor;

        public override void BuildContextualMenu(ContextualMenuPopulateEvent contextualMenuEvent)
        {
            contextualMenuEvent.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
            contextualMenuEvent.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectOutputPorts());

            base.BuildContextualMenu(contextualMenuEvent);
        }

        public virtual void Initialize(string nodeName, GraphView dsGraphView, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();
            DialogueName = nodeName;

            SetPosition(new Rect(position, Vector2.zero));

            graphView = dsGraphView;
            defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            /* TITLE CONTAINER */

            TextField dialogueNameTextField = UtilityElement.CreateTextField(DialogueName, null, callback =>
            {
                TextField target = (TextField)callback.target;
                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(target.value))
                {
                    if (!string.IsNullOrEmpty(DialogueName))
                    {
                        ++graphView.NameErrorsAmount;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(DialogueName))
                    {
                        --graphView.NameErrorsAmount;
                    }
                }

                if (Group == null)
                {
                    graphView.RemoveUngroupedNode(this);
                    DialogueName = target.value;
                    graphView.AddUngroupedNode(this);
                    return;
                }

                Group currentGroup = Group;
                graphView.RemoveGroupedNode(this, Group);
                DialogueName = target.value;
                graphView.AddGroupedNode(this, currentGroup);
            });

            dialogueNameTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__filename-text-field"
            );

            titleContainer.Insert(0, dialogueNameTextField);
        }

        protected Port CreateAnswerPort(object userData, List<AnswerSaveData> answers)
        {
            Port answerPort = this.CreatePort();
            answerPort.ClearClassList();
            answerPort.AddToClassList("ds-node__answer-port");
            answerPort.userData = userData;
            AnswerSaveData answerData = (AnswerSaveData)userData;
            if (answerData.Conditions == null)
            {
                answerData.Conditions = new List<AnswerConditionSaveData>();
            }

            Foldout answerFoldout = UtilityElement.CreateFoldout("Answer");

            Box localizationBox = UtilityElement.CreateLocalizationBox(answerData.LocalizationSaveData, "ds-node__string-dropdown");
            answerPort.Add(localizationBox);

            Button deleteAnswerButton = UtilityElement.CreateButton("X", () =>
            {
                if (answers.Count == 1)
                {
                    return;
                }
                if (answerPort.connected)
                {
                    graphView.DeleteElements(answerPort.connections);
                }
                answers.Remove(answerData);

                outputContainer.Remove(answerFoldout);
                graphView.RemoveElement(answerPort);
            });
            deleteAnswerButton.AddToClassList("ds-node__button");
            answerPort.Add(deleteAnswerButton);


            Foldout conditionsFoldout = UtilityElement.CreateFoldout("Conditions", true);
            conditionsFoldout.AddClasses("ds-node__foldout-right");

            Button addConditionButton = UtilityElement.CreateButton("Add condition", () =>
            {
                AnswerConditionSaveData conditionData = new AnswerConditionSaveData()
                {
                    AnswerConditionType = AnswerConditionTypes.QuestAccepted
                };
                answerData.Conditions.Add(conditionData);
                CreateConditionField(conditionsFoldout, conditionData);
            });
            conditionsFoldout.Add(addConditionButton);

            foreach (AnswerConditionSaveData conditionData in answerData.Conditions)
            {
                CreateConditionField(conditionsFoldout, conditionData);
            }

            answerFoldout.Add(answerPort);
            answerFoldout.Add(conditionsFoldout);

            outputContainer.Add(answerFoldout);

            return answerPort;
        }

        private void CreateConditionField(Foldout foldout, AnswerConditionSaveData conditionData)
        {
            Foldout conditionFoldout = UtilityElement.CreateFoldout($"{conditionData.AnswerConditionType}", true);
            Box box = new Box();

            DropdownField dropdownField = UtilityElement.CreateDropdownField(typeof(AnswerConditionTypes), conditionData.AnswerConditionType.ToString(), null, callback =>
            {
                conditionData.AnswerConditionType = Enum.Parse<AnswerConditionTypes>(callback.newValue);
                conditionFoldout.text = $"{conditionData.AnswerConditionType}";
                box.Clear();
                box = CreateConditionCheckField(box, conditionData);
            });

            Toggle reverseToggle = UtilityElement.CreateBoolField(conditionData.Reversed, "Reversed", callback => conditionData.Reversed = callback.newValue);

            box = CreateConditionCheckField(box, conditionData);

            conditionFoldout.Add(dropdownField);
            conditionFoldout.Add(reverseToggle);
            conditionFoldout.Add(box);
            foldout.Add(conditionFoldout);
        }

        private Box CreateConditionCheckField(Box box, AnswerConditionSaveData conditionData)
        {
            switch (conditionData.AnswerConditionType)
            {
                case AnswerConditionTypes.CharacteristicCheck:
                    conditionData.Skill = SkillsNames.No;
                    conditionData.Quest = null;

                    DropdownField dropdownField = UtilityElement.CreateDropdownField(
                        typeof(Characteristics),
                        conditionData.Characteristic.ToString(),
                        null,
                        callback =>
                        {
                            conditionData.Characteristic = (Characteristics)Enum.Parse(typeof(Characteristics), callback.newValue);
                        }
                    );
                    Label label = new Label()
                    {
                        text = "more or equal"
                    };
                    label.AddToClassList("ds-node__label");
                    TextField valueTextField = UtilityElement.CreateTextField(
                        conditionData.RequiredValue.ToString(),
                        null,
                        callback => conditionData.RequiredValue = int.Parse(callback.newValue)
                    );
                    box.Add(dropdownField);
                    box.Add(label);
                    box.Add(valueTextField);

                    break;
                case AnswerConditionTypes.SkillCheck:
                    conditionData.Quest = null;

                    dropdownField = UtilityElement.CreateDropdownField(
                        typeof(SkillsNames),
                        conditionData.Skill.ToString(),
                        null,
                        callback => conditionData.Skill = (SkillsNames)Enum.Parse(typeof(SkillsNames), callback.newValue)
                    );
                    label = new Label()
                    {
                        text = "more or equal"
                    };
                    label.AddToClassList("ds-node__label");
                    valueTextField = UtilityElement.CreateTextField(
                        conditionData.RequiredValue.ToString(),
                        null,
                        callback => conditionData.RequiredValue = int.Parse(callback.newValue)
                    );
                    box.Add(dropdownField);
                    box.Add(label);
                    box.Add(valueTextField);

                    break;
                case AnswerConditionTypes.QuestAccepted:
                case AnswerConditionTypes.QuestCompleted:
                case AnswerConditionTypes.QuestFinished:
                    conditionData.Skill = SkillsNames.No;

                    ObjectField objectField = UtilityElement.CreateObjectField(
                        typeof(Quest),
                        conditionData.Quest,
                        null,
                        callback => conditionData.Quest = (Quest)callback.newValue
                    );
                    box.Add(objectField);

                    break;
                default:
                    break;
            }
            return box;
        }

        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }

        private void DisconnectInputPorts()
        {
            DisconnectPorts(inputContainer);
        }

        private void DisconnectOutputPorts()
        {
            DisconnectPorts(outputContainer);
        }

        private void DisconnectPorts(VisualElement container)
        {
            foreach (VisualElement port in container.Children())
            {
                if (port.GetType() == typeof(Port))
                {
                    if (!((Port)port).connected)
                    {
                        continue;
                    }
                    graphView.DeleteElements(((Port)port).connections);
                }
            }
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = defaultBackgroundColor;
        }

        public virtual void SaveToGraph(GraphSaveDataScriptableObject graphData)
        {
            BaseNodeSaveData nodeData = new BaseNodeSaveData()
            {
                ID = ID,
                Name = DialogueName,
                GroupID = Group?.ID,
                DialogueType = DialogueType,
                Position = GetPosition().position,
                Answers = Answers
            };

            if (Group != null)
            {
                Group.Nodes.Add(nodeData);
            }

            graphData.StartNodes.Add(nodeData);
        }

        public virtual DialogueScriptableObject SaveToSO(string folderPath)
        {
            DialogueScriptableObject dialogue;

            if (Group != null)
            {
                dialogue = UtilityIO.CreateAsset<DialogueScriptableObject>($"{folderPath}/Groups/{Group.title}/Dialogues", DialogueName);
            }
            else
            {
                dialogue = UtilityIO.CreateAsset<DialogueScriptableObject>($"{folderPath}/Global/Dialogues", DialogueName);
            }

            dialogue.Initialize(
                DialogueName,
                UtilityIO.ConvertNodeAnswersToDialogueAnswers(Answers),
                DialogueType
            );

            UtilityIO.SaveAsset(dialogue);
            return dialogue;
        }
    }
}