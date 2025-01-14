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

        public override void Apply(CharacterCombatManager targetCharacterCombatManager)
        {
            int randomInt = UnityEngine.Random.Range(0, 100);
            if(_chance < randomInt)
            {
                return;
            }
            Action<int> action = null;

            switch (_damageType)
            {
                case DamageType.Physical:
                    action = (int value) => targetCharacterCombatManager.TakePhysicalDamage(value);
                    break;
                case DamageType.Magical:
                    action = (int value) => targetCharacterCombatManager.TakeMagicalDamage(value);
                    break;
                case DamageType.True:
                    action = (int value) => targetCharacterCombatManager.TakeTrueDamage(value);
                    break;
                default:
                    break;
            }
            if (_roundsCount > 1)
            {
                targetCharacterCombatManager.SetPeriodicalChanges(_damageValue, _roundsCount, EffectIcon, action);
                return;
            }
            action(_damageValue);
        }

        private void OnEnable()
        {
            SelfUsable = false;
        }
    }
}
