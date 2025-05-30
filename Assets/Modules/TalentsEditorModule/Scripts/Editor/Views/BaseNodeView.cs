using System;
using System.Collections.Generic;

using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.TalentsModule.ScriptableObjects;

using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    [Serializable]
    public class BaseNodeView : Node
    {
        private Color _defaultBackgroundColor;

        [field: SerializeField] public string ID { get; protected set; }
        [field: SerializeField] public string NodeName { get; protected set; }
        [field: SerializeField] public LocalizationData DescriptionLocalization { get; protected set; }
        [field: SerializeField] public int Cost { get; protected set; }
        [field: SerializeField] public List<Port> InputPorts { get; protected set; }
        [field: SerializeField] public List<Port> OutputPorts { get; protected set; }
        [field: SerializeField] public List<string> OutputConnections { get; protected set; }
        [field: SerializeField] public Vector2 Position { get; protected set; }

        private bool _isExpanded;

        public event EventHandler<NodeNameChangedEventArgs> NodeNameTextFieldChanged;
        public EventHandler<LocalizationDataChangedEventArgs> DescriptionLocalizationFieldChanged;
        public event EventHandler<CostChangedEventArgs> CostTextFieldChanged;
        public event EventHandler PortDisconnected;

        public event EventHandler<SavedToSOEventArgs<TalentScriptableObject>> SavedToSO;

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

        public void Initialize(string id, string nodeName, Vector2 position, Sprite nodeSprite)
        {
            SetPosition(new Rect(position, Vector2.zero));

            ID = id;
            NodeName = nodeName;

            DescriptionLocalization = new LocalizationData("CharacterDescriptions", "Slime", "Slime");
            InputPorts = new List<Port>();
            OutputPorts = new List<Port>();

            _defaultBackgroundColor = new Color(29f / 255f, 29f / 255f, 30f / 255f);

            mainContainer.AddToClassList("ds-node__main-container");
            extensionContainer.AddToClassList("ds-node__extension-container");

            _isExpanded = false;

            Box nodeImage = new Box();
            Background background = Background.FromSprite(nodeSprite);
            nodeImage.style.backgroundImage = background;
            nodeImage.AddToClassList("ds-node__node-image");
            nodeImage.RegisterCallback<MouseUpEvent>(OnSpriteClicked);

            topContainer.AddToClassList("ds-node__ports");
            topContainer.parent.Remove(topContainer);
            nodeImage.Add(topContainer);

            Insert(0, nodeImage);
            Remove(mainContainer);
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

        public virtual TalentScriptableObject SaveToSO(string folderPath, Rect graphRect)
        {
            TalentScriptableObject talentSO;

            talentSO = UtilityIO.CreateAsset<TalentScriptableObject>($"{folderPath}/Talents", NodeName);
            Vector2 positionPercentages = CalculateLocalPositionPercentages(graphRect);
            talentSO.SetPositionPercentages(positionPercentages);

            SavedToSO?.Invoke(this, new SavedToSOEventArgs<TalentScriptableObject>(talentSO));
            return talentSO;
        }

        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }

        protected void OnSpriteClicked(MouseUpEvent evt)
        {
            if (evt.button != 1)
            {
                return;
            }

            evt.StopImmediatePropagation();
            if (_isExpanded)
            {
                Remove(mainContainer);
            }
            else
            {
                Add(mainContainer);
            }
            _isExpanded = !_isExpanded;
        }

        protected void Load(BaseNodeView node)
        {
            ID = node.ID;
            NodeName = node.NodeName;
            DescriptionLocalization = node.DescriptionLocalization;
            Cost = node.Cost;
            OutputConnections = node.OutputConnections;
        }

        protected virtual Port CreateInputPort()
        {
            return null;
        }

        protected virtual Port CreateOutputPort()
        {
            return null;
        }

        protected virtual void CostChanged(CostChangedEventArgs e)
        {
            CostTextFieldChanged?.Invoke(this, e);
        }

        protected virtual void DescriptionChanged(LocalizationDataChangedEventArgs e)
        {
            DescriptionLocalizationFieldChanged?.Invoke(this, e);
        }

        protected Vector2 CalculateLocalPositionPercentages(Rect graphRect)
        {
            return new Vector2(Math.Abs(Position.x - graphRect.xMin) * 100 / (graphRect.size.x - localBound.size.x), Math.Abs(Position.y - graphRect.yMin) * 100 / (graphRect.size.y - localBound.size.y));
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