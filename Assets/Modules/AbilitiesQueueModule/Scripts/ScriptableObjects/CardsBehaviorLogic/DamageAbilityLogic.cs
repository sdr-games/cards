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

        public override void Apply(CharacterCombatManager characterCombatManager)
        {
            int randomInt = UnityEngine.Random.Range(0, 100);
            if(_chance < randomInt)
            {
                return;
            }
            Action action = null;

            switch (_damageType)
            {
                case DamageType.Physical:
                    action = () => characterCombatManager.TakePhysicalDamage(_damageValue);
                    break;
                case DamageType.Magical:
                    action = () => characterCombatManager.TakeMagicalDamage(_damageValue);
                    break;
                case DamageType.True:
                    action = () => characterCombatManager.TakeTrueDamage(_damageValue);
                    break;
                default:
                    break;
            }
            if (_roundsCount > 1)
            {
                characterCombatManager.SetPeriodicalChanges(_damageValue, _roundsCount, EffectIcon, action);
                return;
            }
            action();
        }

        private void OnEnable()
        {
            SelfUsable = false;
        }
    }
}
