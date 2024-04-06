using System.Collections.Generic;

using SDRGames.Whist.TalentsEditorModule.Managers;
using SDRGames.Whist.TalentsEditorModule.Presenters;

using UnityEditor.Experimental.GraphView;

using UnityEngine;

using static SDRGames.Whist.TalentsModule.ScriptableObjects.TalentScriptableObject;

namespace SDRGames.Whist.TalentsEditorModule
{
    public class NodesSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private GraphManager _graphView;

        public void Initialize(GraphManager graphView)
        {
            _graphView = graphView;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            Texture2D indentationIcon = new Texture2D(1, 1);
            indentationIcon.SetPixel(0, 0, Color.clear);
            indentationIcon.Apply();

            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node")),
                new SearchTreeGroupEntry(new GUIContent("Nodes"), 1),
                new SearchTreeEntry(new GUIContent("Astra node", indentationIcon))
                {
                    userData = NodeTypes.Astra,
                    level = 2
                },
                new SearchTreeEntry(new GUIContent("Talamus node", indentationIcon))
                {
                    userData = NodeTypes.Talamus,
                    level = 2
                }
            };

            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = _graphView.GetLocalMousePosition(context.screenMousePosition, true);

            switch (SearchTreeEntry.userData)
            {
                case NodeTypes.Astra:
                {
                    _graphView.AddElement(_graphView.CreateNode<AstraNodePresenter>("Astra", localMousePosition));
                    return true;
                }

                case NodeTypes.Talamus:
                {
                    _graphView.AddElement(_graphView.CreateNode<TalamusNodePresenter>("Talamus", localMousePosition));
                    return true;
                }

                default:
                {
                    return false;
                }
            }
        }
    }
}
