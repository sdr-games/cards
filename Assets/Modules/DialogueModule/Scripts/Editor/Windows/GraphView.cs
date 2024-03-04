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

        private SerializableDictionary<string, NodeErrorData> _ungroupedNodes;
        private SerializableDictionary<string, GroupErrorData> _groups;
        private SerializableDictionary<UnityEditor.Experimental.GraphView.Group, SerializableDictionary<string, NodeErrorData>> _groupedNodes;

        private int _nameErrorsAmount;

        public int NameErrorsAmount
        {
            get
            {
                return _nameErrorsAmount;
            }

            set
            {
                _nameErrorsAmount = value;

                if (_nameErrorsAmount == 0)
                {
                    _editorWindow.EnableSaving();
                }

                if (_nameErrorsAmount == 1)
                {
                    _editorWindow.DisableSaving();
                }
            }
        }

        public GraphView(DialogueEditorWindow dsEditorWindow)
        {
            _editorWindow = dsEditorWindow;

            _ungroupedNodes = new SerializableDictionary<string, NodeErrorData>();
            _groups = new SerializableDictionary<string, GroupErrorData>();
            _groupedNodes = new SerializableDictionary<UnityEditor.Experimental.GraphView.Group, SerializableDictionary<string, NodeErrorData>>();

            AddManipulators();
            AddGridBackground();
            AddMiniMap();

            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();
            OnGroupRenamed();
            OnGraphViewChanged();

            AddStyles();
            AddMiniMapStyles();
        }

        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            this.AddManipulator(CreateNodeContextualMenu("Add Start Node", DialogueTypes.Start));
            this.AddManipulator(CreateNodeContextualMenu("Add Speech Node", DialogueTypes.Speech));

            this.AddManipulator(CreateGroupContextualMenu());
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, DialogueTypes dialogueType)
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

        private IManipulator CreateGroupContextualMenu()
        {
            return new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(
                    "Add Group",
                    actionEvent => CreateGroup(
                        "Group",
                        GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)
                    )
                )
            );
        }

        public Group CreateGroup(string title, Vector2 position)
        {
            Group group = new Group(title, position);

            AddGroup(group);
            AddElement(group);

            foreach (GraphElement selectedElement in selection)
            {
                if (selectedElement is BaseNode)
                {
                    group.AddElement(selectedElement as BaseNode);
                }
            }

            return group;
        }

        public BaseNode CreateNode(string nodeName, DialogueTypes dialogueType, Vector2 position, bool shouldDraw = true)
        {
            Type nodeType = Type.GetType($"SDRGames.Islands.DialogueSystem.Editor.{dialogueType}Node");
            BaseNode node = (BaseNode)Activator.CreateInstance(nodeType);
            node.Initialize(nodeName, this, position);
            AddUngroupedNode(node);
            if (selection.Count > 0)
            {
                foreach (GraphElement selectedElement in selection)
                {
                    if (selectedElement is Group)
                    {
                        Group group = selectedElement as Group;
                        group.AddElement(node);
                    }
                }
            }
            if (shouldDraw)
            {
                node.Draw();
            }
            return node;
        }

        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                Type groupType = typeof(Group);
                Type edgeType = typeof(Edge);

                List<Group> groupsToDelete = new List<Group>();
                List<BaseNode> nodesToDelete = new List<BaseNode>();
                List<Edge> edgesToDelete = new List<Edge>();

                foreach (GraphElement selectedElement in selection)
                {
                    if (selectedElement is BaseNode node)
                    {
                        nodesToDelete.Add(node);
                        continue;
                    }

                    if (selectedElement.GetType() == edgeType)
                    {
                        Edge edge = (Edge)selectedElement;
                        edgesToDelete.Add(edge);
                        continue;
                    }

                    if (selectedElement.GetType() != groupType)
                    {
                        continue;
                    }

                    Group group = (Group)selectedElement;
                    groupsToDelete.Add(group);
                }

                foreach (Group groupToDelete in groupsToDelete)
                {
                    List<BaseNode> groupNodes = new List<BaseNode>();
                    foreach (GraphElement groupElement in groupToDelete.containedElements)
                    {
                        if (!(groupElement is BaseNode))
                        {
                            continue;
                        }

                        BaseNode groupNode = (BaseNode)groupElement;
                        groupNodes.Add(groupNode);
                    }
                    groupToDelete.RemoveElements(groupNodes);
                    RemoveGroup(groupToDelete);
                    RemoveElement(groupToDelete);
                }
                DeleteElements(edgesToDelete);

                foreach (BaseNode nodeToDelete in nodesToDelete)
                {
                    if (nodeToDelete.Group != null)
                    {
                        nodeToDelete.Group.RemoveElement(nodeToDelete);
                    }
                    RemoveUngroupedNode(nodeToDelete);
                    nodeToDelete.DisconnectAllPorts();
                    RemoveElement(nodeToDelete);
                }
            };
        }

        private void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is BaseNode))
                    {
                        continue;
                    }

                    Group dsGroup = (Group)group;
                    BaseNode node = (BaseNode)element;

                    RemoveUngroupedNode(node);
                    AddGroupedNode(node, dsGroup);
                }
            };
        }

        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is BaseNode))
                    {
                        continue;
                    }

                    Group dsGroup = (Group)group;
                    BaseNode node = (BaseNode)element;

                    RemoveGroupedNode(node, dsGroup);
                    AddUngroupedNode(node);
                }
            };
        }

        private void OnGroupRenamed()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                Group dsGroup = (Group)group;
                dsGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();
                if (string.IsNullOrEmpty(dsGroup.title))
                {
                    if (!string.IsNullOrEmpty(dsGroup.OldTitle))
                    {
                        ++NameErrorsAmount;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(dsGroup.OldTitle))
                    {
                        --NameErrorsAmount;
                    }
                }
                RemoveGroup(dsGroup);
                dsGroup.OldTitle = dsGroup.title;
                AddGroup(dsGroup);
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
                        choiceData.NodeID = nextNode.ID;
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    Type edgeType = typeof(Edge);
                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        if (element.GetType() != edgeType)
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

        public void AddUngroupedNode(BaseNode node)
        {
            string nodeName = node.DialogueName.ToLower();

            if (!_ungroupedNodes.ContainsKey(nodeName))
            {
                NodeErrorData nodeErrorData = new NodeErrorData();
                nodeErrorData.Nodes.Add(node);
                _ungroupedNodes.Add(nodeName, nodeErrorData);
                return;
            }

            List<BaseNode> ungroupedNodesList = _ungroupedNodes[nodeName].Nodes;
            ungroupedNodesList.Add(node);
            Color errorColor = _ungroupedNodes[nodeName].Color;
            node.SetErrorStyle(errorColor);

            if (ungroupedNodesList.Count == 2)
            {
                ++NameErrorsAmount;
                ungroupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveUngroupedNode(BaseNode node)
        {
            string nodeName = node.DialogueName.ToLower();

            List<BaseNode> ungroupedNodesList = _ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Remove(node);

            node.ResetStyle();

            if (ungroupedNodesList.Count == 1)
            {
                --NameErrorsAmount;

                ungroupedNodesList[0].ResetStyle();

                return;
            }

            if (ungroupedNodesList.Count == 0)
            {
                _ungroupedNodes.Remove(nodeName);
            }
        }

        private void AddGroup(Group group)
        {
            string groupName = group.title.ToLower();

            if (!_groups.ContainsKey(groupName))
            {
                GroupErrorData groupErrorData = new GroupErrorData();
                groupErrorData.Groups.Add(group);
                _groups.Add(groupName, groupErrorData);
                return;
            }

            List<Group> groupsList = _groups[groupName].Groups;
            groupsList.Add(group);
            Color errorColor = _groups[groupName].Color;
            group.SetErrorStyle(errorColor);
            if (groupsList.Count == 2)
            {
                ++NameErrorsAmount;
                groupsList[0].SetErrorStyle(errorColor);
            }
        }

        private void RemoveGroup(Group group)
        {
            string oldGroupName = group.OldTitle.ToLower();

            List<Group> groupsList = _groups[oldGroupName].Groups;

            groupsList.Remove(group);

            group.ResetStyle();

            if (groupsList.Count == 1)
            {
                --NameErrorsAmount;

                groupsList[0].ResetStyle();

                return;
            }

            if (groupsList.Count == 0)
            {
                _groups.Remove(oldGroupName);
            }
        }

        public void AddGroupedNode(BaseNode node, Group group)
        {
            string nodeName = node.DialogueName.ToLower();
            node.Group = group;

            if (!_groupedNodes.ContainsKey(group))
            {
                _groupedNodes.Add(group, new SerializableDictionary<string, NodeErrorData>());
            }

            if (!_groupedNodes[group].ContainsKey(nodeName))
            {
                NodeErrorData nodeErrorData = new NodeErrorData();
                nodeErrorData.Nodes.Add(node);
                _groupedNodes[group].Add(nodeName, nodeErrorData);
                return;
            }

            List<BaseNode> groupedNodesList = _groupedNodes[group][nodeName].Nodes;
            groupedNodesList.Add(node);
            Color errorColor = _groupedNodes[group][nodeName].Color;
            node.SetErrorStyle(errorColor);

            if (groupedNodesList.Count == 2)
            {
                ++NameErrorsAmount;
                groupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveGroupedNode(BaseNode node, Group group)
        {
            string nodeName = node.DialogueName.ToLower();

            node.Group = null;

            List<BaseNode> groupedNodesList = _groupedNodes[group][nodeName].Nodes;

            groupedNodesList.Remove(node);

            node.ResetStyle();

            if (groupedNodesList.Count == 1)
            {
                --NameErrorsAmount;

                groupedNodesList[0].ResetStyle();

                return;
            }

            if (groupedNodesList.Count == 0)
            {
                _groupedNodes[group].Remove(nodeName);

                if (_groupedNodes[group].Count == 0)
                {
                    _groupedNodes.Remove(group);
                }
            }
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

            _groups.Clear();
            _groupedNodes.Clear();
            _ungroupedNodes.Clear();

            NameErrorsAmount = 0;
        }

        public void ToggleMiniMap()
        {
            _miniMap.visible = !_miniMap.visible;
        }
    }
}