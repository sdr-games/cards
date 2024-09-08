using System;

using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "DamageAbilityLogic", menuName = "SDRGames/Combat/Cards/Damage Ability Logic")]
    public class DamageAbilityLogic : AbilityLogicScriptableObject
    {
        private enum DamageType { Physical, Magical, True };
        [field: SerializeField] private DamageType _damageType;

        [field: SerializeField] public int DamageValue { get; private set; }
        [field: SerializeField] public int RoundsCount { get; private set; }

        public override void Apply(CharacterCombatManager characterCombatManager)
        {
            switch (_damageType)
            {
                case DamageType.Physical:
                    if (RoundsCount > 1)
                    {
                        characterCombatManager.SetPeriodicalChanges(DamageValue, RoundsCount, () => characterCombatManager.TakePhysicalDamage(DamageValue));
                        break;
                    }
                    characterCombatManager.TakePhysicalDamage(DamageValue);
                    break;
                case DamageType.Magical:
                default:
                    if (RoundsCount > 1)
                    {
                        characterCombatManager.SetPeriodicalChanges(DamageValue, RoundsCount, () => characterCombatManager.TakeMagicalDamage(DamageValue));
                        break;
                    }
                    characterCombatManager.TakeMagicalDamage(DamageValue);
                    break;
                case DamageType.True:
                    if (RoundsCount > 1)
                    {
                        characterCombatManager.SetPeriodicalChanges(DamageValue, RoundsCount, () => characterCombatManager.TakeTrueDamage(DamageValue));
                        break;
                    }
                    characterCombatManager.TakeTrueDamage(DamageValue);
                    break;
            }
        }

        private void OnEnable()
        {
            SelfUsable = false;
        }
    }
}
