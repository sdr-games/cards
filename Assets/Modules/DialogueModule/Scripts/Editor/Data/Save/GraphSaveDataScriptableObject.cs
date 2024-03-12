using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Editor.Models;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public class GraphSaveDataScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; private set; }
        [field: SerializeField] public BaseData StartNode { get; private set; }
        [field: SerializeField] public List<AnswerData> AnswerNodes { get; private set; }
        [field: SerializeField] public List<SpeechData> SpeechNodes { get; private set; }
        [field: SerializeField] public List<string> OldNodeNames { get; private set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;

            StartNode = new BaseData("Start", Vector2.zero);
            AnswerNodes = new List<AnswerData>();
            SpeechNodes = new List<SpeechData>();
        }

        public void SetStartNode(BaseData baseData)
        {
            StartNode = baseData;
        }

        public void SetOldNodeNames(List<string> oldNodeNames)
        {
            OldNodeNames = oldNodeNames;
        }
    }
}