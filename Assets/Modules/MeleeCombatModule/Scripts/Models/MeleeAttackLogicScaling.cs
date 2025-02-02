using System;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.MeleeCombatModule.Models
{
    [Serializable]
    public class MeleeAttackLogicScaling
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
