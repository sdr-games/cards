using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Editor.Views;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    [Serializable]
    public class GraphSaveDataScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; private set; }
        [field: SerializeField] public StartNodeView StartNode { get; private set; }
        [field: SerializeField] public List<AnswerNodeView> AnswerNodes { get; private set; }
        [field: SerializeField] public List<SpeechNodeView> SpeechNodes { get; private set; }

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

        public void AddAnswerNode(AnswerNodeView answerNodeView)
        {
            if(!AnswerNodes.Contains(answerNodeView))
            {
                AnswerNodes.Add(answerNodeView);
            }
        }

        public void AddSpeechNode(SpeechNodeView speechNodeView)
        {
            if(!SpeechNodes.Contains(speechNodeView))
            {
                SpeechNodes.Add(speechNodeView);
            }
        }
    }
}