using System;

using SDRGames.Whist.CharacterModule.Models;

using UnityEngine;

namespace SDRGames.Whist.ChronotopMapModule.Models
{
    [Serializable]
    public class ChronotopMapFightPinModel
    {
        [field: SerializeField] public string PrefightText { get; private set; }
        [field: SerializeField] public CommonCharacterParamsModel EnemyCharacterParams { get; private set; }

        //TODO: add dialogue
        //TODO: add loot and rewards
    }
}
