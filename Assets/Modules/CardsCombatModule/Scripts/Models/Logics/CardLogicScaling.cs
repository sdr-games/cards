using System;

using SDRGames.Whist.CardsCombatModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Models
{
    [Serializable]
    public class CardLogicScaling
    {
        [field: SerializeField] public CardLogicScriptableObject CardLogicScriptableObject { get; private set; }
        [SerializeField] private int _multiplier;
        [SerializeField] private int _perLevels;

        public int CalculateMultiplier(int currentLevel)
        {
            return currentLevel % _perLevels * _multiplier;
        }
    }
}
