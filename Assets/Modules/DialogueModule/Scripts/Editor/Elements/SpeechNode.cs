using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Models;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

using static SDRGames.Whist.DialogueSystem.ScriptableObjects.DialogueScriptableObject;
using static SDRGames.Whist.DialogueSystem.ScriptableObjects.DialogueSpeechScriptableObject;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public class SpeechNode : BaseNode
    {
        public LocalizationSaveData LocalizationSaveData { get; set; }
        public Quest Quest { get; set; }
        public DialogueQuestActions DialogueQuestAction { get; set; }
        public DialogueActions DialogueAction { get; set; }

        public override void Initialize(string nodeName, GraphView dsGraphView, Vector2 position)
        {
            base.Initialize(nodeName, dsGraphView, position);

            DialogueType = DialogueTypes.Speech;
            Answers = new List<AnswerSaveData>();
            LocalizationSaveData = new LocalizationSaveData("", "", "");

            AnswerSaveData answerData = new AnswerSaveData()
            {
                LocalizationSaveData = new LocalizationSaveData("", "Answer", "Answer"),
                Conditions = new List<AnswerConditionSaveData>()
            };
            Answers.Add(answerData);
        }

        public override void Draw()
        {
            base.Draw();
            string defaultText = "Answer";

            /* INPUT CONTAINER */

            Port inputPort = this.CreatePort("Answer Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);

            /* MAIN CONTAINER */

            Button addAnswerButton = UtilityElement.CreateButton("Add answer", () =>
            {
                AnswerSaveData answerData = new AnswerSaveData()
                {
                    LocalizationSaveData = new LocalizationSaveData("", defaultText, defaultText),
                    Conditions = new List<AnswerConditionSaveData>()
                };
                Answers.Add(answerData);
                CreateAnswerPort(answerData, Answers);
            });
            addAnswerButton.AddToClassList("ds-node__button");

            mainContainer.Insert(1, addAnswerButton);


            /* OUTPUT CONTAINER */

            foreach (AnswerSaveData answer in Answers)
            {
                CreateAnswerPort(answer, Answers);
            }

            /* EXTENSION CONTAINER */

            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFoldout = UtilityElement.CreateFoldout("Speech Text", true);

            Box localizationBox = UtilityElement.CreateLocalizationBox(LocalizationSaveData);
            textFoldout.Add(localizationBox);

            Foldout questFoldout = UtilityElement.CreateFoldout("Quest", true);

            DropdownField questActionDropdown = UtilityElement.CreateDropdownField(
                typeof(DialogueQuestActions),
                DialogueQuestAction.ToString(),
                null,
                callback => DialogueQuestAction = Enum.Parse<DialogueQuestActions>(callback.newValue)
            );

            ObjectField questObjectField = UtilityElement.CreateObjectField(
                typeof(Quest),
                Quest,
                null,
                callback => Quest = callback.newValue as Quest);

            questFoldout.Add(questActionDropdown);
            questFoldout.Add(questObjectField);

            Foldout actionFoldout = UtilityElement.CreateFoldout("Action", true);

            DropdownField actionDropdown = UtilityElement.CreateDropdownField(
                typeof(DialogueActions),
                DialogueAction.ToString(),
                null,
                callback => DialogueAction = Enum.Parse<DialogueActions>(callback.newValue));

            actionFoldout.Add(actionDropdown);

            customDataContainer.Add(textFoldout);
            customDataContainer.Add(questFoldout);
            customDataContainer.Add(actionFoldout);

            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }

        public override void SaveToGraph(GraphSaveDataScriptableObject graphData)
        {
            List<AnswerSaveData> answers = CloneNodeAnswers(Answers);

            SpeechNodeSaveData nodeData = new SpeechNodeSaveData()
            {
                ID = ID,
                Name = DialogueName,
                GroupID = Group?.ID,
                DialogueType = DialogueType,
                Position = GetPosition().position,
                Answers = answers,
                DialogueAction = DialogueAction,
                DialogueQuestAction = DialogueQuestAction,
                Quest = Quest,
                LocalizationSaveData = LocalizationSaveData
            };

            if (Group != null)
            {
                Group.Nodes.Add(nodeData);
            }

            graphData.SpeechNodes.Add(nodeData);
        }

        private static List<AnswerSaveData> CloneNodeAnswers(List<AnswerSaveData> nodeAnswers)
        {
            List<AnswerSaveData> answers = new List<AnswerSaveData>();

            foreach (AnswerSaveData nodeAnswer in nodeAnswers)
            {
                AnswerSaveData answerData = new AnswerSaveData()
                {
                    LocalizationSaveData = nodeAnswer.LocalizationSaveData,
                    NodeID = nodeAnswer.NodeID,
                    Conditions = nodeAnswer.Conditions
                };

                answers.Add(answerData);
            }

            return answers;
        }

        public override DialogueScriptableObject SaveToSO(string folderPath)
        {
            DialogueSpeechScriptableObject dialogue;

            if (Group != null)
            {
                dialogue = UtilityIO.CreateAsset<DialogueSpeechScriptableObject>($"{folderPath}/Groups/{Group.title}/Dialogues", DialogueName);
            }
            else
            {
                dialogue = UtilityIO.CreateAsset<DialogueSpeechScriptableObject>($"{folderPath}/Global/Dialogues", DialogueName);
            }

            dialogue.Initialize(
                DialogueName,
                new DialogueLocalizationData(LocalizationSaveData.SelectedLocalizationTable, LocalizationSaveData.SelectedEntryKey),
                UtilityIO.ConvertNodeAnswersToDialogueAnswers(Answers),
                DialogueType,
                null,
                DialogueQuestActions.No,
                DialogueActions.No
            );

            UtilityIO.SaveAsset(dialogue);
            return dialogue;
        }
    }
}