using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Editor.Models;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public class GraphSaveDataScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<BaseData> StartNodes { get; set; }
        [field: SerializeField] public List<AnswerData> AnswerNodes { get; set; }
        [field: SerializeField] public List<SpeechData> SpeechNodes { get; set; }
        [field: SerializeField] public List<string> OldNodeNames { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;

            StartNodes = new List<BaseData>();
            AnswerNodes = new List<AnswerData>();
            SpeechNodes = new List<SpeechData>();
        }
    }
}