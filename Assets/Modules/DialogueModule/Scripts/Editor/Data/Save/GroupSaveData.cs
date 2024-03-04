using System;
using System.Collections.Generic;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    [Serializable]
    public class GroupSaveData
    {
        [field: SerializeField] public string ID { get; set; }
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Vector2 Position { get; set; }
        [field: SerializeField] public List<BaseNodeSaveData> Nodes { get; set; }
    }
}