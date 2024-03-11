using System;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.DialogueSystem.Editor.Models;
using SDRGames.Whist.DialogueSystem.Editor.Presenters;
using SDRGames.Whist.DialogueSystem.Editor.Views;
using SDRGames.Whist.DialogueSystem.Helpers;

using UnityEditor;
using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

namespace SDRGames.Whist.DialogueSystem.Editor.Managers
{
    public class GraphManager : GraphView
    {
        public enum NodeTypes { Start = 0, Speech = 1, Answer = 2 };

        private DialogueEditorWindow _editorWindow;
        private MiniMap _miniMap;
        private SerializableDictionary<string, NodeErrorData> _nodes;
        private int _nameErrorsAmount;
        private bool _startNodeExists;

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

        public GraphManager(DialogueEditorWindow editorWindow)
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

        public GraphElement CreateNode<T>(string nodeName, Vector2 position, bool shouldDraw = true) where T : BaseNodePresenter, new()
        {
            if(typeof(T) != typeof(StartNodePresenter) && !_startNodeExists)
            {
                EditorUtility.DisplayDialog(
                    "Start node missing",
                    "Start node is missing, it was created now instead",
                    "OK"
                );
                GraphElement startNode = CreateNode<StartNodePresenter>("Start", position, shouldDraw);
                _startNodeExists = true;
                return startNode;
            }
            else if(typeof(T) == typeof(StartNodePresenter))
            {
                if (_startNodeExists)
                {
                    EditorUtility.DisplayDialog(
                        "Start node already exists",
                        "Start node is already exists, speech node was created now instead",
                        "OK"
                    );
                    return CreateNode<SpeechNodePresenter>("Speech", position, shouldDraw);
                }
                _startNodeExists = true;
            }

            T presenter = new T();
            presenter.Initialize($"{nodeName}", position);

            BaseNodeView baseNodeView = presenter.GetNodeView();
            baseNodeView.NodeNameTextFieldChanged += OnNodeNameTextFieldChanged;
            baseNodeView.AnswerPortRemoved += OnAnswerPortRemoved;
            baseNodeView.PortDisconnected += (object sender, EventArgs e) => DeleteElements(((Port)sender).connections);

            AddNode($"{nodeName.ToLower()}", baseNodeView);

            if (shouldDraw)
            {
                baseNodeView.Draw();
            }

            return baseNodeView;
        }

        public void AddNode(string nodeName, BaseNodeView node)
        {
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
            _startNodeExists = false;
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

            this.AddManipulator(CreateNodeContextualMenu<StartNodePresenter>($"{NodeTypes.Start}"));
            this.AddManipulator(CreateNodeContextualMenu<SpeechNodePresenter>($"{NodeTypes.Speech}"));
            this.AddManipulator(CreateNodeContextualMenu<AnswerNodePresenter>($"{NodeTypes.Answer}"));
        }

        private IManipulator CreateNodeContextualMenu<T>(string nodeName) where T : BaseNodePresenter, new()
        {
            return new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(
                    $"Add {nodeName} Node",
                    actionEvent => AddElement(
                        CreateNode<T>(
                            nodeName,
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
                List<BaseNodeView> nodesToDelete = new List<BaseNodeView>();
                List<Edge> edgesToDelete = new List<Edge>();

                foreach (GraphElement selectedElement in selection)
                {
                    if (selectedElement.GetType() == typeof(StartNodeView))
                    {
                        continue;
                    }

                    if (selectedElement is Edge edge)
                    {
                        edgesToDelete.Add(edge);
                        continue;
                    }

                    nodesToDelete.Add((BaseNodeView)selectedElement);
                }

                DeleteElements(edgesToDelete);

                foreach (BaseNodeView nodeToDelete in nodesToDelete)
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
                        //if(edge.input.node is SpeechNodeView speechNode)
                        //{
                        //    SpeechData speechNodeSaveData = speechNode.SaveData;
                        //    ((AnswerNodeView)edge.output.node).SaveData.AddNextSpeech(speechNodeSaveData);
                        //    continue;
                        //}

                        //if(edge.input.node is AnswerNodeView answerNode)
                        //{
                        //    AnswerData answerNodeSaveData = answerNode.SaveData;
                        //    ((SpeechNodeView)edge.output.node).SaveData.AddAnswer(answerNodeSaveData);
                        //    continue;
                        //}
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
                        //AnswerSaveData choiceData = (AnswerSaveData)edge.output.userData;
                        //choiceData.NodeID = "";
                    }
                }
                return changes;
            };
        }

        public void RemoveNode(BaseNodeView node, string nodeName = "")
        {
            if(string.IsNullOrEmpty(nodeName))
            {
                nodeName = node.NodeName.ToLower();
            }
            List<BaseNodeView> nodes = _nodes[nodeName].Nodes;

            nodes.Remove(node);
            node.ResetStyle();

            if (nodes.Count == 0)
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
            RemoveNode(args.NewNode, args.OldNodeName.ToLower());
            AddNode(args.NewNode.NodeName.ToLower(), args.NewNode);
        }

        private void OnAnswerPortRemoved(object sender, EventArgs e)
        {
            Port answerPort = (Port)sender;
            if (answerPort.connected)
            {
                DeleteElements(answerPort.connections);
            }
            RemoveElement(answerPort);
        }

        private void CheckNodeNameErrors(NodeErrorData nodeErrorData)
        {
            List<BaseNodeView> errorsNodesList = nodeErrorData.Nodes;
            if (errorsNodesList.Count < 2)
            {
                errorsNodesList[0].ResetStyle();
                --NameErrorsAmount;
                return;
            }

            Color errorColor = nodeErrorData.Color;
            foreach (BaseNodeView node in errorsNodesList)
            {
                ++NameErrorsAmount;
                node.SetErrorStyle(errorColor);
            }
        }
    }
}