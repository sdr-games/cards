using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Editor.Managers;

using UnityEditor.Experimental.GraphView;
using UnityEditor.Localization.Plugins.XLIFF.V12;

using UnityEngine;
using UnityEngine.UIElements;

namespace SDRGames.Whist.DialogueSystem.Editor.Views
{
    public class PortView : Port
    {
        protected PortView(Orientation portOrientation, Direction portDirection, Capacity portCapacity, Type type) : base(portOrientation, portDirection, portCapacity, type)
        {

        }

        public new static Port Create<TEdge>(
            Orientation orientation,
            Direction direction,
            Capacity capacity,
            Type type)
            where TEdge : Edge, new()
        {
            DefaultEdgeConnectorListener listener = new DefaultEdgeConnectorListener();
            PortView ele = new PortView(orientation, direction, capacity, type)
            {
                m_EdgeConnector = new EdgeConnector<TEdge>(listener)
            };
            ele.AddManipulator(ele.m_EdgeConnector);
            return ele;
        }

        private class DefaultEdgeConnectorListener : IEdgeConnectorListener
        {
            private GraphViewChange _graphViewChange;
            private NodesSearchWindow _searchWindow;
            private List<Edge> _edgesToCreate;
            private List<GraphElement> _edgesToDelete;

            public DefaultEdgeConnectorListener()
            {
                _edgesToCreate = new List<Edge>();
                _edgesToDelete = new List<GraphElement>();
                _graphViewChange.edgesToCreate = _edgesToCreate;
            }

            public void OnDropOutsidePort(Edge edge, Vector2 position)
            {
                GraphManager graphManager = FindGraphManagerParent(edge);
                if(graphManager == null)
                {
                    return;
                }

                if (_searchWindow == null)
                {
                    _searchWindow = ScriptableObject.CreateInstance<NodesSearchWindow>();
                    _searchWindow.Initialize(graphManager);
                }
                SearchWindow.Open(new SearchWindowContext(position), _searchWindow);
            }

            public void OnDrop(GraphView graphView, Edge edge)
            {
                _edgesToCreate.Clear();
                _edgesToCreate.Add(edge);
                _edgesToDelete.Clear();
                if (edge.input.capacity == Capacity.Single)
                {
                    foreach (Edge connection in edge.input.connections)
                    {
                        if (connection != edge)
                            _edgesToDelete.Add(connection);
                    }
                }

                if (edge.output.capacity == Capacity.Single)
                {
                    foreach (Edge connection in edge.output.connections)
                    {
                        if (connection != edge)
                            _edgesToDelete.Add(connection);
                    }
                }

                if (_edgesToDelete.Count > 0)
                    graphView.DeleteElements(_edgesToDelete);
                List<Edge> edgesToCreate = _edgesToCreate;
                if (graphView.graphViewChanged != null)
                    edgesToCreate = graphView.graphViewChanged(_graphViewChange).edgesToCreate;
                foreach (Edge edge1 in edgesToCreate)
                {
                    graphView.AddElement(edge1);
                    edge.input.Connect(edge1);
                    edge.output.Connect(edge1);
                }
            }

            private GraphManager FindGraphManagerParent(VisualElement element)
            {
                if(element.parent != null && element.parent.GetType() != typeof(GraphManager))
                {
                    return FindGraphManagerParent(element.parent);
                }
                return element.parent as GraphManager;
            }
        }
    }
}
