using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.DialogueModule.Editor.Managers;
using SDRGames.Whist.DialogueModule.Editor.Presenters;

using UnityEditor.Experimental.GraphView;

using UnityEngine;

using static SDRGames.Whist.DialogueModule.Editor.Managers.GraphManager;

namespace SDRGames.Whist.DialogueModule.Editor
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
                new SearchTreeEntry(new GUIContent("Speech node", indentationIcon))
                {
                    userData = NodeTypes.Speech,
                    level = 2
                },
                new SearchTreeEntry(new GUIContent("Answer node", indentationIcon))
                {
                    userData = NodeTypes.Answer,
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
                case NodeTypes.Speech:
                {
                    _graphView.AddElement(_graphView.CreateNode<SpeechNodePresenter>("Speech", localMousePosition));
                    return true;
                }

                case NodeTypes.Answer:
                {
                    _graphView.AddElement(_graphView.CreateNode<AnswerNodePresenter>("Answer", localMousePosition));
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
