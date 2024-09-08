using System;
using System.Collections;
using System.Collections.Generic;
using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "RestorationAbilityLogic", menuName = "SDRGames/Combat/Cards/Restoration Ability Logic")]
    public class RestorationAbilityLogic : AbilityLogicScriptableObject
    {
        private enum RestorationType { Armor, Barrier, Health };
        [field: SerializeField] private RestorationType _restorationType;

        [field: SerializeField] public int RestorationValue { get; private set; }
        [field: SerializeField] public int RoundsCount { get; private set; }

        public override void Apply(CharacterCombatManager characterCombatManager)
        {
            switch (_restorationType)
            {
                case RestorationType.Armor:
                    if (RoundsCount > 1)
                    {
                        characterCombatManager.SetPeriodicalChanges(RestorationValue, RoundsCount, () => characterCombatManager.RestoreArmor(RestorationValue));
                        break;
                    }
                    characterCombatManager.RestoreArmor(RestorationValue);
                    break;
                case RestorationType.Barrier:
                default:
                    if (RoundsCount > 1)
                    {
                        characterCombatManager.SetPeriodicalChanges(RestorationValue, RoundsCount, () => characterCombatManager.RestoreBarrier(RestorationValue));
                        break;
                    }
                    characterCombatManager.RestoreBarrier(RestorationValue);
                    break;
                case RestorationType.Health:
                    if (RoundsCount > 1)
                    {
                        characterCombatManager.SetPeriodicalChanges(RestorationValue, RoundsCount, () => characterCombatManager.RestoreHealth(RestorationValue));
                        break;
                    }
                    characterCombatManager.RestoreHealth(RestorationValue);
                    break;
            }
        }

        private void OnEnable()
        {
            SelfUsable = true;
        }
    }
}
