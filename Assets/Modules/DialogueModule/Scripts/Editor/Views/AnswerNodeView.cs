using System;

using SDRGames.Whist.DialogueSystem.Models;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

namespace SDRGames.Whist.DialogueSystem.Editor.Views
{
    public class AnswerNodeView : BaseNodeView
    {
        [field: SerializeField] public LocalizationData CharacterNameLocalization { get; private set; }
        [field: SerializeField] public LocalizationData TextLocalization { get; private set; }
        [field: SerializeField] public string NextSpeechNodeID { get; private set; }

        public new event EventHandler<SavedToSOEventArgs<DialogueAnswerScriptableObject>> SavedToSO;
        public event EventHandler AnswerPortRemoved;

        public void Initialize(string id, string nodeName, Vector2 position, LocalizationData characterNameLocalization, LocalizationData textLocalization)
        {
            base.Initialize(id, nodeName, position);

            CharacterNameLocalization = characterNameLocalization;
            TextLocalization = textLocalization;

            CreateInputPort();
            CreateOutputPort();
        }

        public void SetNextSpeechNodeID(string speechNodeViewID)
        {
            NextSpeechNodeID = speechNodeViewID;
        }

        public void UnsetNextSpeechNodeID()
        {
            NextSpeechNodeID = "";
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

            Foldout characterNameFoldout = UtilityElement.CreateFoldout("Character Name", true);

            Box characterNameLocalizationBox = UtilityElement.CreateLocalizationBox(CharacterNameLocalization);
            characterNameFoldout.Add(characterNameLocalizationBox);

            Foldout textFoldout = UtilityElement.CreateFoldout("Answer Text", true);

            Box localizationBox = UtilityElement.CreateLocalizationBox(TextLocalization);
            textFoldout.Add(localizationBox);

            customDataContainer.Add(characterNameFoldout);
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
            outputContainer.Add(port);

            return port;
        }
    }
}
