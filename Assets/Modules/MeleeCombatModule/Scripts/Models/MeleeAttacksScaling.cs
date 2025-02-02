using System;
using System.Linq;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.MeleeCombatModule.Models
{
    [Serializable]
    public class MeleeAttacksScaling
    {
        public static MeleeAttacksScaling Instance { get; private set; }

        [SerializeField] private MeleeAttackLogicScaling[] _meleeAttacksLogicScalings;

        public void UpdateStaticFields()
        {
            if (Instance == null)
            {
                Instance = new MeleeAttacksScaling();
            }

            Instance._meleeAttacksLogicScalings = _meleeAttacksLogicScalings;
        }

        public int GetScalingMultiplier(AbilityLogicScriptableObject abilityLogicScriptableObject, int currentLevel)
        {
            MeleeAttackLogicScaling meleeAttackLogicScaling = _meleeAttacksLogicScalings.FirstOrDefault(item => item.AbilityLogicScriptableObject == abilityLogicScriptableObject);
            return meleeAttackLogicScaling.CalculateMultiplier(currentLevel);
        }
    }
}
