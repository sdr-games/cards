using System;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.TalentsEditorModule.Models;
using SDRGames.Whist.TalentsEditorModule.Presenters;
using SDRGames.Whist.TalentsEditorModule.Views;
using SDRGames.Whist.HelpersModule;

using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;
using static SDRGames.Whist.TalentsModule.ScriptableObjects.TalentScriptableObject;

namespace SDRGames.Whist.TalentsEditorModule.Managers
{
    public class GraphManager : GraphView
    {
        private TalentsEditorWindow _editorWindow;
        private NodesSearchWindow _searchWindow;
        private ParametersWindow _parametersWindow;
        private MiniMap _miniMap;
        private SerializableDictionary<string, NodeErrorData> _nodes;
        private SerializableDictionary<BaseNodeView, BaseNodePresenter> _nodesPresenters;
        private int _nameErrorsAmount;

        public BonusesBlackboardWindow BlackboardWindow { get; private set; }

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

        public GraphManager(TalentsEditorWindow editorWindow)
        {
            _editorWindow = editorWindow;

            _nodes = new SerializableDictionary<string, NodeErrorData>();
            _nodesPresenters = new SerializableDictionary<BaseNodeView, BaseNodePresenter>();

            AddManipulators();
            AddGridBackground();
            AddMiniMap();
            AddSearchWindow();
            //AddBlackboard();
            AddParametersWindow();

            OnElementsDeleted();

            AddStyles();
            AddMiniMapStyles();

            this.StretchToParentSize();
        }

        public GraphElement CreateNode<T>(string nodeName, Vector2 position, bool shouldDraw = true) where T : BaseNodePresenter, new()
        {
            T presenter = new T();
            nodeName += _nodes.Count;
            presenter.Initialize($"{nodeName}", position);

            BaseNodeView baseNodeView = presenter.GetNodeView();
            baseNodeView.NodeNameTextFieldChanged += OnNodeNameTextFieldChanged;
            baseNodeView.PortDisconnected += (object sender, EventArgs e) => DeleteElements(((Port)sender).connections);

            AddNode($"{nodeName.ToLower()}", baseNodeView);
            _nodesPresenters.Add(baseNodeView, presenter);

            if (shouldDraw)
            {
                baseNodeView.Draw();
            }

            return baseNodeView;
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

        public void ClearGraph()
        {
            graphElements.ForEach(graphElement => RemoveElement(graphElement));
            _nodes.Clear();
            //BlackboardWindow.Clear();
            _parametersWindow.ClearParameters();
            NameErrorsAmount = 0;
        }

        public void ToggleMiniMap()
        {
            _miniMap.visible = !_miniMap.visible;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList()!.Where(endPort =>
                          endPort.direction != startPort.direction &&
                          endPort.node != startPort.node &&
                          endPort.portType == startPort.portType)
              .ToList();
        }

        public Rect GetGraphRect()
        {
            return CalculateRectToFitAll(contentContainer);
        }

        public Texture2D GetBackgroundImage()
        {
            return _parametersWindow.GetBackgroundImageFieldValue();
        }

        public void SetBackgroundImage(Texture2D backgroundImage)
        {
            _parametersWindow.SetBackgroundImageFieldValue(backgroundImage);
        }
        private void AddNode(string nodeName, BaseNodeView node)
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

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMenu<AstraNodePresenter>($"{NodeTypes.Astra}"));
            this.AddManipulator(CreateNodeContextualMenu<TalamusNodePresenter>($"{NodeTypes.Talamus}"));
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
                    if (selectedElement is Edge edge)
                    {
                        edgesToDelete.Add(edge);
                        continue;
                    }

                    if(selectedElement is BaseNodeView nodeView)
                    {
                        nodesToDelete.Add(nodeView);
                        continue;
                    }
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
                        //if (edge.output.node is StartNodeView startNode)
                        //{
                        //    string inputID = ((BaseNodeView)edge.input.node).ID;
                        //    startNode.SetNextSpeechNodeID(inputID);
                        //    continue;
                        //}

                        //if (edge.output.node is AnswerNodeView answerNode)
                        //{
                        //    string inputID = ((BaseNodeView)edge.input.node).ID;
                        //    answerNode.SetNextSpeechNodeID(inputID);
                        //    continue;
                        //}

                        //if (edge.output.node is SpeechNodeView speechNode)
                        //{
                        //    string inputID = ((BaseNodeView)edge.input.node).ID;
                        //    speechNode.AddConnection(inputID);
                        //    continue;
                        //}
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        //if (element.GetType() != typeof(Edge))
                        //{
                        //    continue;
                        //}
                        //Edge edge = (Edge)element;

                        //if (edge.output.node is StartNodeView startNode)
                        //{
                        //    startNode.UnsetNextSpeechNodeID();
                        //    continue;
                        //}

                        //if (edge.output.node is AnswerNodeView answerNode)
                        //{
                        //    answerNode.UnsetNextSpeechNodeID();
                        //    continue;
                        //}

                        //if (edge.output.node is SpeechNodeView speechNode)
                        //{
                        //    string inputID = ((BaseNodeView)edge.input.node).ID;
                        //    speechNode.RemoveConnection(inputID);
                        //    continue;
                        //}
                    }
                }
                return changes;
            };
        }

        private void RemoveNode(BaseNodeView node, string nodeName = "")
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
                "TalentsEditorStyles/DSGraphViewStyles.uss",
                "TalentsEditorStyles/DSNodeStyles.uss"
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

        private void AddSearchWindow()
        {
            if(_searchWindow == null)
            {
                _searchWindow = ScriptableObject.CreateInstance<NodesSearchWindow>();
                _searchWindow.Initialize(this);
            }
            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }

        private void AddBlackboard()
        {
            BlackboardWindow = new BonusesBlackboardWindow(this);
            Add(BlackboardWindow);
        }

        private void AddParametersWindow()
        {
            _parametersWindow = new ParametersWindow();
            Add(_parametersWindow);
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