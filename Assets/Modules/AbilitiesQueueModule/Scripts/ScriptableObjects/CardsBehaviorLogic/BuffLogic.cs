using System;

using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "BuffAbilityLogic", menuName = "SDRGames/Combat/Logics/Buff Ability Logic")]
    public class BuffLogic : AbilityLogicScriptableObject
    {
        private enum BuffType { HealthPoints, Strength, Agility, Stamina, Intelligence, PhysicalDamage, MagicDamage };
        [SerializeField] private BuffType _buffType;

        [SerializeField] private int _buffValue;
        [SerializeField] private bool _inPercents;

        public override void Apply(CharacterCombatManager characterCombatManager)
        {
            int randomInt = UnityEngine.Random.Range(0, 100);
            if(_chance < randomInt)
            {
                return;
            }
            Action<int> action = null;

            switch (_buffType)
            {
                case BuffType.HealthPoints:
                    action = (int value) => { characterCombatManager.GetParams().HealthPoints.IncreaseTemporaryBonus(value); };
                    break;
                case BuffType.Strength:
                    action = (int value) => { characterCombatManager.GetParams().IncreaseStrength(value); };
                    break;
                case BuffType.Agility:
                    action = (int value) => { characterCombatManager.GetParams().IncreaseAgility(value); };
                    break;
                case BuffType.Stamina:
                    action = (int value) => { characterCombatManager.GetParams().IncreaseStamina(value); };
                    break;
                case BuffType.Intelligence:
                    action = (int value) => { characterCombatManager.GetParams().IncreaseIntelligence(value); };
                    break;
                case BuffType.PhysicalDamage:
                    action = (int value) => { characterCombatManager.GetParams().IncreasePhysicalDamage(value); };
                    break;
                case BuffType.MagicDamage:
                    action = (int value) => { characterCombatManager.GetParams().IncreaseMagicalDamage(value); };
                    break;
                default:
                    break;
            }
            characterCombatManager.SetBuff(
                _buffValue,
                _roundsCount,
                EffectIcon,
                action,
                _inPercents
            );
        }

        private void OnEnable()
        {
            SelfUsable = true;
        }

        private void OnValidate()
        {
            if(_roundsCount < 1)
            {
                _roundsCount = 1;
            }
        }
    }
}
