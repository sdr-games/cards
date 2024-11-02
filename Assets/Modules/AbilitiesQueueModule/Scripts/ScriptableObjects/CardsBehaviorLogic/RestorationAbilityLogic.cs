using System;
using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "RestorationAbilityLogic", menuName = "SDRGames/Combat/Logics/Restoration Ability Logic")]
    public class RestorationAbilityLogic : AbilityLogicScriptableObject
    {
        private enum RestorationType { Armor, Barrier, Health, Stamina, Breath };
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
                        characterCombatManager.SetPeriodicalChanges(RestorationValue, RoundsCount, EffectIcon, () => characterCombatManager.RestoreArmor(RestorationValue));
                        break;
                    }
                    characterCombatManager.RestoreArmor(RestorationValue);
                    break;
                case RestorationType.Barrier:
                default:
                    if (RoundsCount > 1)
                    {
                        characterCombatManager.SetPeriodicalChanges(RestorationValue, RoundsCount, EffectIcon, () => characterCombatManager.RestoreBarrier(RestorationValue));
                        break;
                    }
                    characterCombatManager.RestoreBarrier(RestorationValue);
                    break;
                case RestorationType.Health:
                    if (RoundsCount > 1)
                    {
                        characterCombatManager.SetPeriodicalChanges(RestorationValue, RoundsCount, EffectIcon, () => characterCombatManager.RestoreHealth(RestorationValue));
                        break;
                    }
                    characterCombatManager.RestoreHealth(RestorationValue);
                    break;
                case RestorationType.Stamina:
                    if (RoundsCount > 1)
                    {
                        characterCombatManager.SetPeriodicalChanges(RestorationValue, RoundsCount, EffectIcon, () => characterCombatManager.RestoreStamina(RestorationValue));
                        break;
                    }
                    characterCombatManager.RestoreStamina(RestorationValue);
                    break;
                case RestorationType.Breath:
                    if (RoundsCount > 1)
                    {
                        characterCombatManager.SetPeriodicalChanges(RestorationValue, RoundsCount, EffectIcon, () => characterCombatManager.RestoreBreath(RestorationValue));
                        break;
                    }
                    characterCombatManager.RestoreBreath(RestorationValue);
                    break;
            }
        }

        private void OnEnable()
        {
            SelfUsable = true;
        }
    }
}
