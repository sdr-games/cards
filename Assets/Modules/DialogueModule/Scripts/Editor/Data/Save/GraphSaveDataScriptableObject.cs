using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Helpers;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public class GraphSaveDataScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<GroupSaveData> Groups { get; set; }
        [field: SerializeField] public List<BaseNodeSaveData> StartNodes { get; set; }
        [field: SerializeField] public List<SpeechNodeSaveData> SpeechNodes { get; set; }
        [field: SerializeField] public List<string> OldGroupNames { get; set; }
        [field: SerializeField] public List<string> OldUngroupedNodeNames { get; set; }
        [field: SerializeField] public SerializableDictionary<string, List<string>> OldGroupedNodeNames { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;

            Groups = new List<GroupSaveData>();
            StartNodes = new List<BaseNodeSaveData>();
            SpeechNodes = new List<SpeechNodeSaveData>();
        }
    }
}