using System;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.DialogueSystem.Helpers;

using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

using static SDRGames.Whist.DialogueSystem.ScriptableObjects.DialogueScriptableObject;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public class GraphView : UnityEditor.Experimental.GraphView.GraphView
    {
        private DialogueEditorWindow _editorWindow;
        private MiniMap _miniMap;
        private SerializableDictionary<string, NodeErrorData> _nodes;
        private int _nameErrorsAmount;

        public int NameErrorsAmount
        {
            get => _nameErrorsAmount;
            set
            {
                _nameErrorsAmount = value;

                if(_nameErrorsAmount < 0)
                {
                    _nameErrorsAmount = 0;
                }

                if (_nameErrorsAmount == 0)
                {
                    _editorWindow.EnableSaving();
                    return;
                }
                _editorWindow.DisableSaving();
            }
        }

        public GraphView(DialogueEditorWindow editorWindow)
        {
            _editorWindow = editorWindow;

            _nodes = new SerializableDictionary<string, NodeErrorData>();

            AddManipulators();
            AddGridBackground();
            AddMiniMap();

            OnElementsDeleted();
            OnGraphViewChanged();

            AddStyles();
            AddMiniMapStyles();
        }

        public BaseNode CreateNode(string nodeName, NodeTypes dialogueType, Vector2 position, bool shouldDraw = true)
        {
            Type nodeType = Type.GetType($"{GetType().Namespace}.{dialogueType}Node");
            BaseNode node = (BaseNode)Activator.CreateInstance(nodeType);
            node.Initialize($"{nodeName}", position);
            node.NodeNameTextFieldChanged += OnNodeNameTextFieldChanged;
            node.AnswerPortRemoved += (object sender, EventArgs e) => 
            {
                Port answerPort = (Port)sender;
                if (answerPort.connected)
                {
                    DeleteElements(answerPort.connections);
                }
                RemoveElement(answerPort);
            };
            node.PortDisconnected += (object sender, EventArgs e) => DeleteElements(((Port)sender).connections);
            AddNode(node);

            if (shouldDraw)
            {
                node.Draw();
            }

            return node;
        }

        public void AddNode(BaseNode node)
        {
            string nodeName = node.SaveData.Name.ToLower();

            if (!_nodes.ContainsKey(nodeName))
            {
                NodeErrorData nodeErrorData = new NodeErrorData();
                nodeErrorData.Nodes.Add(node);
                _nodes.Add(nodeName, nodeErrorData);
            }
            else
            {
                _nodes[nodeName].Nodes.Add(node);
            }
            CheckNodeNameErrors(_nodes[nodeName]);
        }

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
            {
                worldMousePosition = _editorWindow.rootVisualElement.ChangeCoordinatesTo(_editorWindow.rootVisualElement.parent, mousePosition - _editorWindow.position.position);
            }
            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
            return localMousePosition;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList()!.Where(endPort =>
                          endPort.direction != startPort.direction &&
                          endPort.node != startPort.node &&
                          endPort.portType == startPort.portType)
              .ToList();
        }

        public void ClearGraph()
        {
            graphElements.ForEach(graphElement => RemoveElement(graphElement));
            _nodes.Clear();
            NameErrorsAmount = 0;
        }

        public void ToggleMiniMap()
        {
            _miniMap.visible = !_miniMap.visible;
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMenu("Add Start Node", NodeTypes.Start));
            this.AddManipulator(CreateNodeContextualMenu("Add Speech Node", NodeTypes.Speech));
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, NodeTypes dialogueType)
        {
            return new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(
                    actionTitle,
                    actionEvent => AddElement(
                        CreateNode(
                            dialogueType.ToString(),
                            dialogueType,
                            GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)
                        )
                    )
                )
            );
        }

        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                List<BaseNode> nodesToDelete = new List<BaseNode>();
                List<Edge> edgesToDelete = new List<Edge>();

                foreach (GraphElement selectedElement in selection)
                {
                    if (selectedElement is BaseNode node)
                    {
                        continue;
                    }

                    if (selectedElement is SpeechNode speechNode)
                    {
                        nodesToDelete.Add(speechNode);
                        continue;
                    }

                    if (selectedElement is Edge edge)
                    {
                        edgesToDelete.Add(edge);
                        continue;
                    }
                }

                DeleteElements(edgesToDelete);

                foreach (BaseNode nodeToDelete in nodesToDelete)
                {
                    nodeToDelete.DisconnectAllPorts();
                    RemoveNode(nodeToDelete);
                    RemoveElement(nodeToDelete);
                }
            };
        }

        private void OnGraphViewChanged()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        BaseNode nextNode = (BaseNode)edge.input.node;
                        AnswerSaveData choiceData = (AnswerSaveData)edge.output.userData;
                        choiceData.NodeID = nextNode.SaveData.ID;
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        if (element.GetType() != typeof(Edge))
                        {
                            continue;
                        }
                        Edge edge = (Edge)element;
                        AnswerSaveData choiceData = (AnswerSaveData)edge.output.userData;
                        choiceData.NodeID = "";
                    }
                }
                return changes;
            };
        }

        public void RemoveNode(BaseNode node, string nodeName = "")
        {
            if(string.IsNullOrEmpty(nodeName))
            {
                nodeName = node.SaveData.Name.ToLower();
            }
            List<BaseNode> nodesList = _nodes[nodeName].Nodes;

            nodesList.Remove(node);
            node.ResetStyle();

            if (nodesList.Count == 0)
            {
                _nodes.Remove(nodeName);
                return;
            }

            CheckNodeNameErrors(_nodes[nodeName]);
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        private void AddMiniMap()
        {
            _miniMap = new MiniMap()
            {
                anchored = true
            };
            _miniMap.SetPosition(new Rect(15, 50, 200, 180));
            Add(_miniMap);
            _miniMap.visible = false;
        }

        private void AddStyles()
        {
            this.AddStyleSheets(
                "DialogueSystem/DSGraphViewStyles.uss",
                "DialogueSystem/DSNodeStyles.uss"
            );
        }

        private void AddMiniMapStyles()
        {
            StyleColor backgroundColor = new StyleColor(new Color32(29, 29, 30, 255));
            StyleColor borderColor = new StyleColor(new Color32(51, 51, 51, 255));

            _miniMap.style.backgroundColor = backgroundColor;
            _miniMap.style.borderTopColor = borderColor;
            _miniMap.style.borderRightColor = borderColor;
            _miniMap.style.borderBottomColor = borderColor;
            _miniMap.style.borderLeftColor = borderColor;
        }

        private Vector2 GetViewportCenter()
        {
            return new Vector2
            (
                contentViewContainer.worldBound.x / 2,
                contentViewContainer.worldBound.y / 2
            );
        }

        private void OnNodeNameTextFieldChanged(object sender, NodeNameChangedEventArgs args)
        {
            if (args.NewNode == null)
            {
                return;
            }
            string nodeName = args.OldNodeName.ToLower();
            RemoveNode(args.NewNode, nodeName);
            AddNode(args.NewNode);
        }

        private void CheckNodeNameErrors(NodeErrorData nodeErrorData)
        {
            List<BaseNode> errorsNodesList = nodeErrorData.Nodes;
            if (errorsNodesList.Count < 2)
            {
                errorsNodesList[0].ResetStyle();
                --NameErrorsAmount;
                return;
            }

            Color errorColor = nodeErrorData.Color;
            foreach (BaseNode node in errorsNodesList)
            {
                ++NameErrorsAmount;
                node.SetErrorStyle(errorColor);
            }
        }
    }
}