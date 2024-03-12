using System.Collections.Generic;

using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

using static SDRGames.Whist.DialogueSystem.Editor.Managers.GraphManager;

namespace SDRGames.Whist.DialogueSystem.Editor.Views
{
    public class StartNodeView : BaseNodeView
    {
        private List<SpeechNodeView> _relationships;

        public override void Initialize(string id, string nodeName, Vector2 position)
        {
            base.Initialize(id, nodeName, position);

            _relationships = new List<SpeechNodeView>();

            CreateAnswerPort(typeof(AnswerNodeView), NodeTypes.Answer);
        }

        public override void Draw()
        {
            /* MAIN CONTAINER */
            Button addAnswerButton = UtilityElement.CreateButton("Add answer", () =>
            {
                CreateAnswerPort(typeof(AnswerNodeView), NodeTypes.Answer);
            });
            addAnswerButton.AddToClassList("ds-node__button");
            mainContainer.Insert(1, addAnswerButton);

            /* OUTPUT CONTAINER */

            foreach (Port port in OutputPorts)
            {
                outputContainer.Add(port);
            }
            RefreshExpandedState();

            base.Draw();
        }

        public void CreateRelationShip(SpeechNodeView speechNodeView)
        {
            if(!_relationships.Contains(speechNodeView))
            {
                _relationships.Add(speechNodeView);
            }
        }

        public void DeleteRelationShip(SpeechNodeView speechNodeView)
        {
            if (_relationships.Contains(speechNodeView))
            {
                _relationships.Remove(speechNodeView);
            }
        }
    }
}