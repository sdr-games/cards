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
        [SerializeField] private LocalizationData _characterNameLocalization;
        [SerializeField] private LocalizationData _textLocalization;
        [SerializeField] private List<string> _connections;

        public new event EventHandler<SavedToSOEventArgs<DialogueSpeechScriptableObject>> SavedToSO;

        public void Initialize(string id, string nodeName, Vector2 position, LocalizationData characterNameLocalization, LocalizationData textLocalization)
        {
            base.Initialize(id, nodeName, position);

            _characterNameLocalization = characterNameLocalization;
            _textLocalization = textLocalization;

            CreateInputPort(typeof(AnswerNodeView), NodeTypes.Answer);
            CreateOutputPort(typeof(SpeechNodeView), NodeTypes.Speech);
        }

        public override void Draw()
        {
            base.Draw();

            /* INPUT CONTAINER */

            inputContainer.Add(InputPorts[0]);

            /* MAIN CONTAINER */

            Button addAnswerButton = UtilityElement.CreateButton("Add answer", () =>
            {
                CreateOutputPort(typeof(SpeechNodeView), NodeTypes.Speech);
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

        public override void SaveToGraph(GraphSaveDataScriptableObject graphData)
        {
            base.SaveToGraph(graphData);
            foreach (PortView port in OutputPorts)
            {
                foreach (Edge edge in port.connections)
                {
                    string id = ((BaseNodeView)edge.input.node).ID;
                    if (_connections.Contains(id))
                    {
                        continue;
                    }
                    _connections.Add(id);
                }
            }
            graphData.SpeechNodes.Add(this);
        }

        public override DialogueScriptableObject SaveToSO(string folderPath)
        {
            DialogueSpeechScriptableObject dialogueSO;

            dialogueSO = UtilityIO.CreateAsset<DialogueSpeechScriptableObject>($"{folderPath}/Dialogues", NodeName);

            SavedToSO?.Invoke(this, new SavedToSOEventArgs<DialogueSpeechScriptableObject>(dialogueSO));
            return dialogueSO;
        }

        //public void LoadData(SpeechData nodeData, List<AnswerSaveData> answers)
        //{
        //    base.LoadData(nodeData);
        //    SaveData.SetCharacterNameLocalization(nodeData.CharacterNameLocalization);
        //    SaveData.SetID(nodeData.ID);
        //}
    }
}