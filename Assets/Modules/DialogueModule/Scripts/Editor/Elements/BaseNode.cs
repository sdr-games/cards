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
    public abstract class BaseNode : Node
    {
        private Color _defaultBackgroundColor;

        public BaseNodeSaveData SaveData { get; protected set; }

        public event EventHandler<NodeNameChangedEventArgs> NodeNameTextFieldChanged;
        public event EventHandler AnswerPortRemoved;
        public event EventHandler PortDisconnected;


        public override void BuildContextualMenu(ContextualMenuPopulateEvent contextualMenuEvent)
        {
            contextualMenuEvent.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
            contextualMenuEvent.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectOutputPorts());

            base.BuildContextualMenu(contextualMenuEvent);
        }

        public virtual void Initialize(string nodeName, Vector2 position)
        {
            List<AnswerSaveData> answers = new List<AnswerSaveData>();

            AnswerSaveData answerData = new AnswerSaveData()
            {
                LocalizationSaveData = new LocalizationSaveData("", "Answer", "Answer"),
                Conditions = new List<AnswerConditionSaveData>()
            };
            answers.Add(answerData);

            SetPosition(new Rect(position, Vector2.zero));

            SaveData = new BaseNodeSaveData(nodeName, answers, GetPosition().position);
            SaveData.GenerateID();

            _defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            /* TITLE CONTAINER */
            TextField nodeNameTextField = UtilityElement.CreateTextField(SaveData.Name, null, callback =>
            {
                TextField target = (TextField)callback.target;
                string oldName = SaveData.Name;
                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(target.value))
                {
                    if (!string.IsNullOrEmpty(SaveData.Name))
                    {
                        NodeNameTextFieldChanged?.Invoke(this, new NodeNameChangedEventArgs(oldName, null));
                        return;
                    }
                }

                //SaveData.SetName(target.value);
                SaveData.Name = target.value;
                NodeNameTextFieldChanged?.Invoke(this, new NodeNameChangedEventArgs(oldName, this));
            });

            nodeNameTextField.AddClasses(
                "ds-node__text-field",
                "ds-node__text-field__hidden",
                "ds-node__filename-text-field"
            );

            titleContainer.Insert(0, nodeNameTextField);
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = _defaultBackgroundColor;
        }

        public abstract void SaveToGraph(GraphSaveDataScriptableObject graphData);

        public virtual DialogueScriptableObject SaveToSO(string folderPath)
        {
            DialogueScriptableObject dialogueSO;

            dialogueSO = UtilityIO.CreateAsset<DialogueScriptableObject>($"{folderPath}/Dialogues", SaveData.Name);

            dialogueSO.Initialize(
                SaveData.Name,
                UtilityIO.ConvertNodeAnswersToDialogueAnswers(SaveData.Answers),
                SaveData.NodeType
            );

            UtilityIO.SaveAsset(dialogueSO);
            return dialogueSO;
        }

        public virtual void LoadData(BaseNodeSaveData nodeData, List<AnswerSaveData> answers)
        {
            SaveData = nodeData;
            SaveData.SetAnswers(answers);
        }

        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }

        protected Port CreateAnswerPort(object userData)
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
                if (SaveData.Answers.Count == 1)
                {
                    return;
                }
                SaveData.RemoveAnswer(answerData);

                outputContainer.Remove(answerFoldout);
                AnswerPortRemoved?.Invoke(answerPort, EventArgs.Empty);
            });
            deleteAnswerButton.AddToClassList("ds-node__button");
            answerPort.Add(deleteAnswerButton);


            Foldout conditionsFoldout = UtilityElement.CreateFoldout("Conditions", true);
            conditionsFoldout.AddClasses("ds-node__foldout-right");

            Button addConditionButton = UtilityElement.CreateButton("Add condition", () =>
            {
                AnswerConditionSaveData conditionData = new AnswerConditionSaveData()
                {
                    AnswerConditionType = AnswerConditionTypes.CharacteristicCheck
                };
                answerData.Conditions.Add(conditionData);
                UtilityElement.CreateConditionField(conditionsFoldout, conditionData);
            });
            conditionsFoldout.Add(addConditionButton);

            foreach (AnswerConditionSaveData conditionData in answerData.Conditions)
            {
                UtilityElement.CreateConditionField(conditionsFoldout, conditionData);
            }

            answerFoldout.Add(answerPort);
            answerFoldout.Add(conditionsFoldout);

            outputContainer.Add(answerFoldout);

            return answerPort;
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
                    PortDisconnected?.Invoke(port, EventArgs.Empty);
                }
            }
        }
    }
}