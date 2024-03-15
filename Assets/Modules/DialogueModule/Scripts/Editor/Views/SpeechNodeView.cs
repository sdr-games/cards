using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Models;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

namespace SDRGames.Whist.DialogueSystem.Editor.Views
{
    public class SpeechNodeView : BaseNodeView
    {
        [field: SerializeField] public LocalizationData CharacterNameLocalization { get; private set; }
        [field: SerializeField] public LocalizationData TextLocalization { get; private set; }
        [field: SerializeField] public List<string> Connections { get; private set; }

        public new event EventHandler<SavedToSOEventArgs<DialogueSpeechScriptableObject>> SavedToSO;
        public event EventHandler AnswerPortRemoved;

        public void Initialize(string id, string nodeName, Vector2 position, LocalizationData characterNameLocalization, LocalizationData textLocalization)
        {
            base.Initialize(id, nodeName, position);

            CharacterNameLocalization = characterNameLocalization;
            TextLocalization = textLocalization;
            Connections = new List<string>();

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

            Foldout characterNameFoldout = UtilityElement.CreateFoldout("Character Name", true);

            Box characterNameLocalizationBox = UtilityElement.CreateLocalizationBox(CharacterNameLocalization);
            characterNameFoldout.Add(characterNameLocalizationBox);

            Foldout textFoldout = UtilityElement.CreateFoldout("Speech Text", true);

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
            foreach (PortView port in OutputPorts)
            {
                foreach (Edge edge in port.connections)
                {
                    string id = ((BaseNodeView)edge.input.node).ID;
                    if (Connections.Contains(id))
                    {
                        continue;
                    }
                    Connections.Add(id);
                }
            }
            graphData.AddSpeechNode(this);
        }

        public override DialogueScriptableObject SaveToSO(string folderPath)
        {
            DialogueSpeechScriptableObject dialogueSO;

            dialogueSO = UtilityIO.CreateAsset<DialogueSpeechScriptableObject>($"{folderPath}/Dialogues", NodeName);

            SavedToSO?.Invoke(this, new SavedToSOEventArgs<DialogueSpeechScriptableObject>(dialogueSO));
            return dialogueSO;
        }

        public void AddConnection(string nodeID)
        {
            if(!Connections.Contains(nodeID))
            {
                Connections.Add(nodeID);
            }
        }

        public void RemoveConnection(string nodeID)
        {
            if (Connections.Contains(nodeID))
            {
                Connections.Remove(nodeID);
            }
        }

        public override Port CreateInputPort()
        {
            Port port = this.CreatePort(typeof(AnswerNodeView), $"Answer Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
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