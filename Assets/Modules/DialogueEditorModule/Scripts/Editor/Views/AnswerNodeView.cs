using System;

using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.DialogueModule.ScriptableObjects;

using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;

using UnityEngine;
using UnityEngine.UIElements;

namespace SDRGames.Whist.DialogueEditorModule.Views
{
    [Serializable]
    public class AnswerNodeView : BaseNodeView
    {
        [field: SerializeField] public CharacterInfoScriptableObject Character { get; private set; }
        [field: SerializeField] public LocalizationData TextLocalization { get; private set; }

        public new event EventHandler<SavedToSOEventArgs<DialogueAnswerScriptableObject>> SavedToSO;
        public event EventHandler<AnswerLoadedEventArgs> Loaded;
        public event EventHandler<CharacterUpdatedEventArgs> CharacterUpdated;

        public void Initialize(string id, string nodeName, Vector2 position)
        {
            base.Initialize(id, nodeName, position);

            TextLocalization = new LocalizationData("", "", "");

            CreateInputPort();
            CreateOutputPort();
        }

        public override void Draw()
        {
            base.Draw();

            /* INPUT CONTAINER */

            inputContainer.Add(InputPorts[0]);

            /* OUTPUT CONTAINER */

            foreach (Port port in OutputPorts)
            {
                outputContainer.Add(port);
            }

            /* EXTENSION CONTAINER */

            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("ds-node__custom-data-container");

            Foldout characterFoldout = UtilityElement.CreateFoldout("Character Name");

            ObjectField characterObjectField = UtilityElement.CreateObjectField(typeof(CharacterInfoScriptableObject), Character, "Character", callback =>
            {
                Character = callback.newValue as CharacterInfoScriptableObject;
                CharacterUpdated?.Invoke(this, new CharacterUpdatedEventArgs(Character));
            });
            characterFoldout.Add(characterObjectField);

            Foldout textFoldout = UtilityElement.CreateFoldout("Answer Text");
            Box localizationBox = UtilityElement.CreateLocalizationBox(TextLocalization, "", TextLocalizationFieldChanged);
            textFoldout.Add(localizationBox);

            customDataContainer.Add(characterFoldout);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);

            RefreshExpandedState();
        }

        public override void SaveToGraph(GraphSaveDataScriptableObject graphData)
        {
            base.SaveToGraph(graphData);
            graphData.AddAnswerNode(this);
        }

        public override DialogueScriptableObject SaveToSO(string folderPath)
        {
            DialogueAnswerScriptableObject dialogueSO;

            dialogueSO = UtilityIO.CreateAsset<DialogueAnswerScriptableObject>($"{folderPath}/Dialogues", NodeName);

            SavedToSO?.Invoke(this, new SavedToSOEventArgs<DialogueAnswerScriptableObject>(dialogueSO));
            return dialogueSO;
        }

        public void Load(AnswerNodeView node)
        {
            base.Load(node);
            Character = node.Character;
            TextLocalization = node.TextLocalization;
            Loaded?.Invoke(this, new AnswerLoadedEventArgs(Character, TextLocalization));
        }

        public override Port CreateInputPort()
        {
            Port port = this.CreatePort(typeof(SpeechNodeView), $"Speech Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            port.ClearClassList();
            port.AddToClassList($"ds-node__speech-input-port");
            InputPorts.Add(port);

            return port;
        }

        public override Port CreateOutputPort()
        {
            Port port = this.CreatePort(typeof(AnswerNodeView), "Answer Connection");
            port.ClearClassList();
            port.AddToClassList($"ds-node__answer-output-port");

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

            return port;
        }
    }
}
