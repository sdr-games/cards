using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Editor.Views;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    [Serializable]
    public class GraphSaveDataScriptableObject : ScriptableObject
    {
        [SerializeField] private string _fileName;
        [SerializeField] private StartNodeView _startNode;
        [SerializeField] private List<AnswerNodeView> _answerNodes;
        [SerializeField] private List<SpeechNodeView> _speechNodes;
        [SerializeField] private List<string> _oldNodeNames;

        public string FileName => _fileName;
        public StartNodeView StartNode => _startNode;
        public List<AnswerNodeView> AnswerNodes => _answerNodes;
        public List<SpeechNodeView> SpeechNodes => _speechNodes;
        public List<string> OldNodeNames => _oldNodeNames;

        private void OnEnable()
        {
            _answerNodes = new List<AnswerNodeView>();
            _speechNodes = new List<SpeechNodeView>();
        }

        public void Initialize(string fileName)
        {
            _fileName = fileName;
        }

        public void SetStartNode(StartNodeView startNode)
        {
            _startNode = startNode;
        }

        public void AddAnswerNode(AnswerNodeView answerNodeView)
        {
            if(!_answerNodes.Contains(answerNodeView))
            {
                _answerNodes.Add(answerNodeView);
            }
        }

        public void AddSpeechNode(SpeechNodeView speechNodeView)
        {
            if(!_speechNodes.Contains(speechNodeView))
            {
                _speechNodes.Add(speechNodeView);
            }
        }

        public void SetOldNodeNames(List<string> oldNodeNames)
        {
            _oldNodeNames = oldNodeNames;
        }
    }
}