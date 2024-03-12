using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Models;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    [Serializable]
    public class AnswerSaveData
    {
        [field: SerializeField] public LocalizationData LocalizationSaveData { get; set; }
        [field: SerializeField] public string NodeID { get; set; }
        [field: SerializeField] public List<AnswerConditionSaveData> Conditions { get; set; }
    }
}