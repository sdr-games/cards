using System;
using System.Collections.Generic;

using UnityEngine;

using static SDRGames.Whist.DialogueSystem.ScriptableObjects.DialogueScriptableObject;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    [Serializable]
    public class BaseNodeSaveData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public List<AnswerSaveData> Answers { get; set; }
        [field: SerializeField] public string GroupID { get; set; }
        [field: SerializeField] public DialogueTypes DialogueType { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
    }
}