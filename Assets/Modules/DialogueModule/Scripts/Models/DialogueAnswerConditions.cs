using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Models
{
    [Serializable]
    public class DialogueAnswerConditions
    {
        [field: SerializeField] public List<DialogueAnswerCondition> Conditions { get; set; }
    }

}