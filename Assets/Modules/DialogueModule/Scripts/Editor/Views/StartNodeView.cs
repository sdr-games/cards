using System;

using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEditor.Experimental.GraphView;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor.Views
{
    public class StartNodeView : BaseNodeView
    {
        [field: SerializeField] public string NextSpeechNodeID { get; private set; }

        public new event EventHandler<SavedToSOEventArgs<DialogueStartScriptableObject>> SavedToSO;
        public event EventHandler AnswerPortRemoved;

        public override void Initialize(string id, string nodeName, Vector2 position)
        {
            base.Initialize(id, nodeName, position);

            CreateOutputPort();
        }

        public void SetNextSpeechNodeID(string speechNodeViewID)
        {
            NextSpeechNodeID = speechNodeViewID;
        }

        public void UnsetNextSpeechNodeID()
        {
            NextSpeechNodeID = "";
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

        public override void LoadData(BaseNodeView node)
        {
            base.LoadData(node);
            NextSpeechNodeID = ((StartNodeView)node).NextSpeechNodeID;
        }

        public override Port CreateOutputPort()
        {
            Port port = this.CreatePort(typeof(AnswerNodeView), "Answer Connection");
            port.ClearClassList();
            port.AddToClassList($"ds-node__answer-output-port");

            OutputPorts.Add(port);
            outputContainer.Add(port);

            return port;
        }
    }
}