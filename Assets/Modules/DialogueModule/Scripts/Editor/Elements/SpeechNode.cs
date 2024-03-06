using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Models;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

using static SDRGames.Whist.DialogueSystem.ScriptableObjects.DialogueScriptableObject;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public class SpeechNode : BaseNode
    {
        public new SpeechNodeSaveData SaveData { get; private set; }

        //public LocalizationSaveData LocalizationSaveData { get; set; }
        //public Quest Quest { get; set; }
        //public DialogueQuestActions DialogueQuestAction { get; set; }
        //public DialogueActions DialogueAction { get; set; }

        public override void Initialize(string nodeName, Vector2 position)
        {
            base.Initialize(nodeName, position);
            LocalizationSaveData localizationSaveData = new LocalizationSaveData("", "", "");
            SaveData = new SpeechNodeSaveData(base.SaveData, localizationSaveData);
            SaveData.SetNodeType(NodeTypes.Speech);
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
                SaveData.AddAnswer(answerData);
                CreateAnswerPort(answerData);
            });
            addAnswerButton.AddToClassList("ds-node__button");

            mainContainer.Insert(1, addAnswerButton);


            /* OUTPUT CONTAINER */

            foreach (AnswerSaveData answer in SaveData.Answers)
            {
                CreateAnswerPort(answer);
            }

            /* EXTENSION CONTAINER */

            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout textFoldout = UtilityElement.CreateFoldout("Speech Text", true);

            Box localizationBox = UtilityElement.CreateLocalizationBox(SaveData.LocalizationSaveData);
            textFoldout.Add(localizationBox);

            //Foldout questFoldout = UtilityElement.CreateFoldout("Quest", true);

            //DropdownField questActionDropdown = UtilityElement.CreateDropdownField(
            //    typeof(DialogueQuestActions),
            //    DialogueQuestAction.ToString(),
            //    null,
            //    callback => DialogueQuestAction = Enum.Parse<DialogueQuestActions>(callback.newValue)
            //);

            //ObjectField questObjectField = UtilityElement.CreateObjectField(
            //    typeof(Quest),
            //    Quest,
            //    null,
            //    callback => Quest = callback.newValue as Quest);

            //questFoldout.Add(questActionDropdown);
            //questFoldout.Add(questObjectField);

            //Foldout actionFoldout = UtilityElement.CreateFoldout("Action", true);

            //DropdownField actionDropdown = UtilityElement.CreateDropdownField(
            //    typeof(DialogueActions),
            //    DialogueAction.ToString(),
            //    null,
            //    callback => DialogueAction = Enum.Parse<DialogueActions>(callback.newValue));

            //actionFoldout.Add(actionDropdown);

            customDataContainer.Add(textFoldout);
            //customDataContainer.Add(questFoldout);
            //customDataContainer.Add(actionFoldout);

            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }

        public override void SaveToGraph(GraphSaveDataScriptableObject graphData)
        {
            SaveData.SetPosition(GetPosition().position);
            graphData.SpeechNodes.Add(SaveData);
        }

        public override DialogueScriptableObject SaveToSO(string folderPath)
        {
            DialogueSpeechScriptableObject dialogueSO;

            dialogueSO = UtilityIO.CreateAsset<DialogueSpeechScriptableObject>($"{folderPath}/Dialogues", SaveData.Name);

            dialogueSO.Initialize(
                SaveData.Name,
                new DialogueLocalizationData(SaveData.LocalizationSaveData.SelectedLocalizationTable, SaveData.LocalizationSaveData.SelectedEntryKey),
                UtilityIO.ConvertNodeAnswersToDialogueAnswers(SaveData.Answers),
                SaveData.NodeType
            );

            UtilityIO.SaveAsset(dialogueSO);
            return dialogueSO;
        }

        public void LoadData(SpeechNodeSaveData nodeData, List<AnswerSaveData> answers)
        {
            base.LoadData(nodeData, answers);
            SaveData.SetLocalizationSaveData(nodeData.LocalizationSaveData);
            SaveData.SetID(nodeData.ID);
        }
    }
}