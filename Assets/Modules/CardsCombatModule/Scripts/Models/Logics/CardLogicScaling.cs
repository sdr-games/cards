using System;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Models
{
    [Serializable]
    public class CardLogicScaling
    {
        [field: SerializeField] public AbilityLogicScriptableObject AbilityLogicScriptableObject { get; private set; }
        [SerializeField] private int _multiplier;
        [SerializeField] private int _perLevels;

        public int CalculateMultiplier(int currentLevel)
        {
            return currentLevel % _perLevels * _multiplier;
        }
    }
}
