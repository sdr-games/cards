using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Editor.Views;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public class GraphSaveDataScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; private set; }
        [field: SerializeField] public StartNodeView StartNode { get; private set; }
        [field: SerializeField] public List<AnswerNodeView> AnswerNodes { get; private set; }
        [field: SerializeField] public List<SpeechNodeView> SpeechNodes { get; private set; }
        [field: SerializeField] public List<string> OldNodeNames { get; private set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;

            AnswerNodes = new List<AnswerNodeView>();
            SpeechNodes = new List<SpeechNodeView>();
        }

        public void SetStartNode(StartNodeView startNode)
        {
            StartNode = startNode;
        }

        public void SetOldNodeNames(List<string> oldNodeNames)
        {
            OldNodeNames = oldNodeNames;
        }
    }
}