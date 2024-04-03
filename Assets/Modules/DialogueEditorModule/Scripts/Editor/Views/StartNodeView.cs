using System;

using SDRGames.Whist.DialogueModule.ScriptableObjects;

using UnityEditor.Experimental.GraphView;

using UnityEngine;

namespace SDRGames.Whist.DialogueEditorModule.Views
{
    public class StartNodeView : BaseNodeView
    {
        public new event EventHandler<SavedToSOEventArgs<DialogueStartScriptableObject>> SavedToSO;

        public override void Initialize(string id, string nodeName, Vector2 position)
        {
            base.Initialize(id, nodeName, position);
            CreateOutputPort();
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

        public override Port CreateOutputPort()
        {
            Port port = this.CreatePort(typeof(AnswerNodeView), "Answer Connection");
            port.ClearClassList();
            port.AddToClassList($"ds-node__answer-output-port");

            OutputPorts.Add(port);

            return port;
        }
    }
}