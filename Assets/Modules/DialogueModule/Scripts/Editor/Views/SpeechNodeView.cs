using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Models;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace SDRGames.Whist.DialogueSystem.Editor.Views
{
    public class SpeechNodeView : BaseNodeView
    {
        private DialogueCharacterScriptableObject _character;
        private LocalizationData _textLocalization;

        public new event EventHandler<SavedToSOEventArgs<DialogueSpeechScriptableObject>> SavedToSO;
        public event EventHandler<CharacterUpdatedEventArgs> CharacterUpdated;

        public void Initialize(string id, string nodeName, Vector2 position, DialogueCharacterScriptableObject character, LocalizationData textLocalization)
        {
            base.Initialize(id, nodeName, position);

            _character = character;
            _textLocalization = textLocalization;

            CreateInputPort();
            CreateOutputPort();
        }

        public override void Draw()
        {
            base.Draw();

            /* INPUT CONTAINER */

            inputContainer.Add(InputPorts[0]);

            /* MAIN CONTAINER */

            //Button addAnswerButton = UtilityElement.CreateButton("Add answer", () =>
            //{
            //    CreateOutputPort();
            //});
            //addAnswerButton.AddToClassList("ds-node__button");

            //mainContainer.Insert(1, addAnswerButton);

            /* OUTPUT CONTAINER */

            foreach (Port port in OutputPorts)
            {
                outputContainer.Add(port);
            }

            /* EXTENSION CONTAINER */

            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout characterFoldout = UtilityElement.CreateFoldout("Character Name");

            ObjectField characterObjectField = UtilityElement.CreateObjectField(typeof(DialogueCharacterScriptableObject), _character, "Character", callback =>
            {
                _character = callback.newValue as DialogueCharacterScriptableObject;
                CharacterUpdated?.Invoke(this, new CharacterUpdatedEventArgs(_character));
            });
            characterFoldout.Add(characterObjectField);

            Foldout textFoldout = UtilityElement.CreateFoldout("Speech Text");

            Box localizationBox = UtilityElement.CreateLocalizationBox(_textLocalization);
            textFoldout.Add(localizationBox);

            customDataContainer.Add(characterFoldout);
            customDataContainer.Add(textFoldout);

            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }

        public override void SaveToGraph(GraphSaveDataScriptableObject graphData)
        {
            base.SaveToGraph(graphData);
            graphData.AddSpeechNode(this);
        }

        public override DialogueScriptableObject SaveToSO(string folderPath)
        {
            DialogueSpeechScriptableObject dialogueSO;

            dialogueSO = UtilityIO.CreateAsset<DialogueSpeechScriptableObject>($"{folderPath}/Dialogues", NodeName);

            SavedToSO?.Invoke(this, new SavedToSOEventArgs<DialogueSpeechScriptableObject>(dialogueSO));
            return dialogueSO;
        }

        public override Port CreateInputPort()
        {
            Port port = this.CreatePort(typeof(AnswerNodeView), $"Answer Connection", Orientation.Horizontal, Direction.Input);
            port.ClearClassList();
            port.AddToClassList($"ds-node__answer-input-port");
            InputPorts.Add(port);

            return port;
        }

        public override Port CreateOutputPort()
        {
            Port port = this.CreatePort(typeof(SpeechNodeView), "Speech Connection");
            port.ClearClassList();
            port.AddToClassList($"ds-node__speech-output-port");

            //Button deleteAnswerButton = UtilityElement.CreateButton("X", () =>
            //{
            //    if (OutputPorts.Count == 1)
            //    {
            //        return;
            //    }
            //    outputContainer.Remove(answerPort);
            //    AnswerPortRemoved?.Invoke(answerPort, EventArgs.Empty);
            //});
            //deleteAnswerButton.AddToClassList("ds-node__button");
            //answerPort.Add(deleteAnswerButton);

            OutputPorts.Add(port);
            outputContainer.Add(port);

            return port;
        }
    }
}