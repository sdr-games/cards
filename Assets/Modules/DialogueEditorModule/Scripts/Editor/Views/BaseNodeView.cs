using System;
using System.Collections.Generic;

using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.DialogueModule.ScriptableObjects;

using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;
using SDRGames.Whist.LocalizationModule.Models;

namespace SDRGames.Whist.DialogueEditorModule.Views
{
    [Serializable]
    public class BaseNodeView : Node
    {
        private Color _defaultBackgroundColor;

        [field: SerializeField] public string ID { get; protected set; }
        [field: SerializeField] public string NodeName { get; protected set; }
        [field: SerializeField] public List<Port> InputPorts { get; protected set; }
        [field: SerializeField] public List<Port> OutputPorts { get; protected set; }
        [field: SerializeField] public List<string> OutputConnections { get; protected set; }
        [field: SerializeField] public Vector2 Position { get; protected set; }

        public event EventHandler<NodeNameChangedEventArgs> NodeNameTextFieldChanged;
        public EventHandler<LocalizationDataChangedEventArgs> TextLocalizationFieldChanged;
        public event EventHandler PortDisconnected;

        public event EventHandler<SavedToSOEventArgs<DialogueScriptableObject>> SavedToSO;

        public override void BuildContextualMenu(ContextualMenuPopulateEvent contextualMenuEvent)
        {
            contextualMenuEvent.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectInputPorts());
            contextualMenuEvent.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectOutputPorts());

            base.BuildContextualMenu(contextualMenuEvent);
        }

        public override Port InstantiatePort(Orientation orientation, Direction direction, Port.Capacity capacity, Type type)
        {
            return PortView.Create<Edge>(orientation, direction, capacity, type);
        }

        public virtual void Initialize(string id, string nodeName, Vector2 position)
        {
            SetPosition(new Rect(position, Vector2.zero));

            ID = id;
            NodeName = nodeName;

            InputPorts = new List<Port>();
            OutputPorts = new List<Port>();

            _defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");
        }

        public virtual void Draw()
        {
            /* TITLE CONTAINER */
            TextField nodeNameTextField = UtilityElement.CreateTextField(NodeName, null, callback =>
            {
                string oldName = NodeName;
                TextField target = (TextField)callback.target;
                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
                NodeName = target.value;
                NodeNameTextFieldChanged?.Invoke(this, new NodeNameChangedEventArgs(oldName, this));
                if (string.IsNullOrEmpty(target.value))
                {
                    return;
                }
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

        public virtual void SaveToGraph(GraphSaveDataScriptableObject graphData) 
        {
            Position = GetPosition().position;
            OutputConnections = new List<string>();
            foreach (Port port in OutputPorts)
            {
                foreach (Edge edge in port.connections)
                {
                    OutputConnections.Add(((BaseNodeView)edge.input.node).ID);
                }
            }
        }

        public virtual DialogueScriptableObject SaveToSO(string folderPath)
        {
            DialogueScriptableObject dialogueSO;

            dialogueSO = UtilityIO.CreateAsset<DialogueScriptableObject>($"{folderPath}/Dialogues", NodeName);

            SavedToSO?.Invoke(this, new SavedToSOEventArgs<DialogueScriptableObject>(dialogueSO));
            return dialogueSO;
        }

        public void Load(BaseNodeView node)
        {
            ID = node.ID;
            NodeName = node.NodeName;
            OutputConnections = node.OutputConnections;
        }

        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }

        public virtual Port CreateInputPort()
        {
            return null;
        }

        public virtual Port CreateOutputPort()
        {
            return null;
        }

        private void DisconnectInputPorts()
        {
            DisconnectPorts(InputPorts);
        }

        private void DisconnectOutputPorts()
        {
            DisconnectPorts(OutputPorts);
        }

        private void DisconnectPorts(List<Port> ports)
        {
            foreach (Port port in ports)
            {
                if (!port.connected)
                {
                    continue;
                }
                PortDisconnected?.Invoke(port, EventArgs.Empty);
            }
        }
    }
}