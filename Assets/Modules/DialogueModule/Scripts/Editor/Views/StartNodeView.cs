using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

namespace SDRGames.Whist.DialogueSystem.Editor.Views
{
    public class StartNodeView : BaseNodeView
    {
        public override void Initialize(string nodeName, Vector2 position)
        {
            base.Initialize(nodeName, position);

            CreateAnswerPort(typeof(AnswerNodeView));
        }

        public override void Draw()
        {
            /* MAIN CONTAINER */
            Button addAnswerButton = UtilityElement.CreateButton("Add answer", () =>
            {
                CreateAnswerPort(typeof(SpeechNodeView));
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

        public override void SaveToGraph(GraphSaveDataScriptableObject graphData)
        {
            //SaveData.SetPosition(GetPosition().position);
            //graphData.StartNodes.Add(SaveData);
        }
    }
}