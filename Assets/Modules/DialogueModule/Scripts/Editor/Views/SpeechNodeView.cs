using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Editor.Models;
using SDRGames.Whist.DialogueSystem.Models;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

using static SDRGames.Whist.DialogueSystem.ScriptableObjects.DialogueScriptableObject;

namespace SDRGames.Whist.DialogueSystem.Editor.Views
{
    public class SpeechNodeView : BaseNodeView
    {
        private LocalizationSaveData _characterNameLocalization;
        private LocalizationSaveData _textLocalization;

        public void Initialize(string nodeName, Vector2 position, LocalizationSaveData characterNameLocalization, LocalizationSaveData textLocalization)
        {
            base.Initialize(nodeName, position);

            _characterNameLocalization = characterNameLocalization;
            _textLocalization = textLocalization;

            CreateAnswerPort(typeof(SpeechNodeView));

            Port inputPort = this.CreatePort(typeof(AnswerNodeView), "Answer Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            InputPorts.Add(inputPort);
        }

        public override void Draw()
        {
            base.Draw();

            /* INPUT CONTAINER */

            inputContainer.Add(InputPorts[0]);

            /* MAIN CONTAINER */

            Button addAnswerButton = UtilityElement.CreateButton("Add answer", () =>
            {
                CreateAnswerPort(typeof(SpeechNodeView));
            });
            addAnswerButton.AddToClassList("ds-node__button");

            mainContainer.Insert(1, addAnswerButton);

            /* OUTPUT CONTAINER */

            foreach (Port port in OutputPorts)
            {
                outputContainer.Add(port);
            }

            /* EXTENSION CONTAINER */

            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout characterNameFoldout = UtilityElement.CreateFoldout("Character Name", true);

            Box characterNameLocalizationBox = UtilityElement.CreateLocalizationBox(_characterNameLocalization);
            characterNameFoldout.Add(characterNameLocalizationBox);

            Foldout textFoldout = UtilityElement.CreateFoldout("Speech Text", true);

            Box localizationBox = UtilityElement.CreateLocalizationBox(_textLocalization);
            textFoldout.Add(localizationBox);

            customDataContainer.Add(characterNameFoldout);
            customDataContainer.Add(textFoldout);

            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }

        //public override void SaveToGraph(GraphSaveDataScriptableObject graphData)
        //{
        //    SaveData.SetPosition(GetPosition().position);
        //    graphData.SpeechNodes.Add(SaveData);
        //}

        //public override DialogueScriptableObject SaveToSO(string folderPath)
        //{
        //    DialogueSpeechScriptableObject dialogueSO;

        //    dialogueSO = UtilityIO.CreateAsset<DialogueSpeechScriptableObject>($"{folderPath}/Dialogues", SaveData.Name);

        //    dialogueSO.Initialize(
        //        SaveData.Name,
        //        new DialogueLocalizationData(SaveData.CharacterNameLocalization.SelectedLocalizationTable, SaveData.CharacterNameLocalization.SelectedEntryKey),
        //        new DialogueLocalizationData(SaveData.TextLocalization.SelectedLocalizationTable, SaveData.TextLocalization.SelectedEntryKey),
        //        UtilityIO.ConvertNodeAnswersToDialogueAnswers(SaveData.Answers),
        //        SaveData.NodeType
        //    );

        //    UtilityIO.SaveAsset(dialogueSO);
        //    return dialogueSO;
        //}

        //public void LoadData(SpeechData nodeData, List<AnswerSaveData> answers)
        //{
        //    base.LoadData(nodeData);
        //    SaveData.SetCharacterNameLocalization(nodeData.CharacterNameLocalization);
        //    SaveData.SetID(nodeData.ID);
        //}
    }
}