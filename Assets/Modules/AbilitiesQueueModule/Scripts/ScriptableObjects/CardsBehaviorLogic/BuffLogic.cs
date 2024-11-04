using System;

using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "BuffAbilityLogic", menuName = "SDRGames/Combat/Logics/Buff Ability Logic")]
    public class BuffLogic : AbilityLogicScriptableObject
    {
        private enum BuffType { HealthPoints, Strength, Agility, PhysicalDamage, MagicDamage };
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
            Action action = null;

            switch (_buffType)
            {
                case BuffType.HealthPoints:
                    action = () => { characterCombatManager.GetParams().HealthPoints.IncreaseBonus(_buffValue); };
                    break;
                case BuffType.Strength:
                case BuffType.Agility:
                    break;
                case BuffType.PhysicalDamage:
                    break;
                case BuffType.MagicDamage:
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
