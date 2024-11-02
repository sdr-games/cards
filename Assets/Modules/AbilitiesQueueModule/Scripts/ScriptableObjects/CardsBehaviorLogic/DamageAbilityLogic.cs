using System;

using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "DamageAbilityLogic", menuName = "SDRGames/Combat/Logics/Damage Ability Logic")]
    public class DamageAbilityLogic : AbilityLogicScriptableObject
    {
        private enum DamageType { Physical, Magical, True };
        [SerializeField] private DamageType _damageType;

        [SerializeField] private int _damageValue;
        [SerializeField] private int _roundsCount;

        public override void Apply(CharacterCombatManager characterCombatManager)
        {
            int randomInt = UnityEngine.Random.Range(0, 100);
            switch (_damageType)
            {
                case DamageType.Physical:
                    if (_chance >= randomInt)
                    {
                        if (_roundsCount > 1)
                        {
                            characterCombatManager.SetPeriodicalChanges(_damageValue, _roundsCount, EffectIcon, () => characterCombatManager.TakePhysicalDamage(_damageValue));
                            break;
                        }
                        characterCombatManager.TakePhysicalDamage(_damageValue);
                    }
                    break;
                case DamageType.Magical:
                default:
                    if (_chance >= randomInt)
                    {
                        if (_roundsCount > 1)
                        {
                            characterCombatManager.SetPeriodicalChanges(_damageValue, _roundsCount, EffectIcon, () => characterCombatManager.TakeMagicalDamage(_damageValue));
                            break;
                        }
                        characterCombatManager.TakeMagicalDamage(_damageValue);
                    }
                    break;
                case DamageType.True:
                    if (_chance >= randomInt)
                    {
                        if (_roundsCount > 1)
                        {
                            characterCombatManager.SetPeriodicalChanges(_damageValue, _roundsCount, EffectIcon, () => characterCombatManager.TakeTrueDamage(_damageValue));
                            break;
                        }
                        characterCombatManager.TakeTrueDamage(_damageValue);
                    }
                    break;
            }
        }

        private void OnEnable()
        {
            SelfUsable = false;
        }
    }
}
