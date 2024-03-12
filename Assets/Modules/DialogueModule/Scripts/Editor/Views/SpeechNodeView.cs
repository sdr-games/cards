using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Helpers;
using SDRGames.Whist.DialogueSystem.Models;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

using static SDRGames.Whist.DialogueSystem.Editor.Managers.GraphManager;

namespace SDRGames.Whist.DialogueSystem.Editor.Views
{
    public class SpeechNodeView : BaseNodeView
    {
        private LocalizationData _characterNameLocalization;
        private LocalizationData _textLocalization;
        private Dictionary<BaseNodeView, BaseNodeView> _relationships;

        public new event EventHandler<SavedToSOEventArgs<DialogueSpeechScriptableObject>> SavedToSO;
        public event EventHandler<RelationshipAddedEventArgs> RelationshipAdded;
        public event EventHandler<RelationshipRemovedEventArgs> RelationshipRemoved;

        public void Initialize(string id, string nodeName, Vector2 position, LocalizationData characterNameLocalization, LocalizationData textLocalization)
        {
            base.Initialize(id, nodeName, position);

            _characterNameLocalization = characterNameLocalization;
            _textLocalization = textLocalization;
            _relationships = new Dictionary<BaseNodeView, BaseNodeView>();

            CreateInputPort(typeof(AnswerNodeView), NodeTypes.Answer);
            CreateAnswerPort(typeof(SpeechNodeView), NodeTypes.Speech);
        }

        public override void Draw()
        {
            base.Draw();

            /* INPUT CONTAINER */

            inputContainer.Add(InputPorts[0]);

            /* MAIN CONTAINER */

            Button addAnswerButton = UtilityElement.CreateButton("Add answer", () =>
            {
                CreateAnswerPort(typeof(SpeechNodeView), NodeTypes.Speech);
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

        public override DialogueScriptableObject SaveToSO(string folderPath)
        {
            DialogueSpeechScriptableObject dialogueSO;

            dialogueSO = UtilityIO.CreateAsset<DialogueSpeechScriptableObject>($"{folderPath}/Dialogues", NodeName);

            SavedToSO?.Invoke(this, new SavedToSOEventArgs<DialogueSpeechScriptableObject>(dialogueSO));
            return dialogueSO;
        }

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