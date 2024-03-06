using System.Collections.Generic;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public class GraphSaveDataScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<BaseNodeSaveData> StartNodes { get; set; }
        [field: SerializeField] public List<SpeechNodeSaveData> SpeechNodes { get; set; }
        [field: SerializeField] public List<string> OldNodeNames { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;

            StartNodes = new List<BaseNodeSaveData>();
            SpeechNodes = new List<SpeechNodeSaveData>();
        }
    }
}