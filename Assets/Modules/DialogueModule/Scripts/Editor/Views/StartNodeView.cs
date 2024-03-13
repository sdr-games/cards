using System;

using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEditor.Experimental.GraphView;

using UnityEngine;

using static SDRGames.Whist.DialogueSystem.Editor.Managers.GraphManager;

namespace SDRGames.Whist.DialogueSystem.Editor.Views
{
    public class StartNodeView : BaseNodeView
    {
        public new event EventHandler<SavedToSOEventArgs<DialogueStartScriptableObject>> SavedToSO;

        public override void Initialize(string id, string nodeName, Vector2 position)
        {
            base.Initialize(id, nodeName, position);

            CreateOutputPort(typeof(AnswerNodeView), NodeTypes.Answer, true);
        }

        public override void Draw()
        {
            /* MAIN CONTAINER */
            //Button addAnswerButton = UtilityElement.CreateButton("Add answer", () =>
            //{
            //    CreateAnswerPort(typeof(AnswerNodeView), NodeTypes.Answer);
            //});
            //addAnswerButton.AddToClassList("ds-node__button");
            //mainContainer.Insert(1, addAnswerButton);

            /* OUTPUT CONTAINER */

            foreach (Port port in OutputPorts)
            {
                outputContainer.Add(port);
            }
            RefreshExpandedState();

            base.Draw();
        }

        public override DialogueScriptableObject SaveToSO(string folderPath)
        {
            DialogueStartScriptableObject dialogueSO;

            dialogueSO = UtilityIO.CreateAsset<DialogueStartScriptableObject>($"{folderPath}/Dialogues", NodeName);

            SavedToSO?.Invoke(this, new SavedToSOEventArgs<DialogueStartScriptableObject>(dialogueSO));
            return dialogueSO;
        }

        public override void SaveToGraph(GraphSaveDataScriptableObject graphData)
        {
            base.SaveToGraph(graphData);
            graphData.SetStartNode(this);
        }
    }
}