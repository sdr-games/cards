using System;

using SDRGames.Whist.CharacterInfoModule.ScriptableObjects;
using SDRGames.Whist.DialogueModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.ChronotopMapModule.Models
{
    [Serializable]
    public class ChronotopMapFightPinModel
    {
        [field: SerializeField] public string PrefightText { get; private set; }
        [field: SerializeField] public CharacterInfoScriptableObject EnemyCharacterParams { get; private set; }
        [field: SerializeField] public DialogueContainerScriptableObject DialogueContainerScriptableObject { get; private set; }
        //TODO: add dialogue
        //TODO: add loot and rewards
    }
}
