using System;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.CharacterModule.ScriptableObjects;

using static SDRGames.Whist.AbilitiesModule.ScriptableObjects.BuffLogicScriptableObject;

namespace SDRGames.Whist.CardsCombatModule.Models
{
    public class BuffLogic : CardLogic
    {
        protected BuffTypes _buffType;
        protected int _buffValue;
        protected bool _inPercents;

        public BuffLogic(BuffLogicScriptableObject buffLogicScriptableObject) : base(buffLogicScriptableObject)
        {
            _buffType = buffLogicScriptableObject.BuffType;
            _buffValue = buffLogicScriptableObject.BuffValue;
            _inPercents = buffLogicScriptableObject.InPercents;
        }

        public override void Apply(CharacterCombatManager characterCombatManager)
        {
            int randomInt = UnityEngine.Random.Range(0, 100);
            if (_chance < randomInt)
            {
                return;
            }
            Action<int> action = null;
            CharacterParamsModel characterParams = characterCombatManager.GetParams();

            switch (_buffType)
            {
                case BuffTypes.HealthPoints:
                    action = (int value) => { characterParams.HealthPoints.IncreaseTemporaryBonus(value); };
                    break;
                case BuffTypes.Strength:
                    action = (int value) => { characterParams.IncreaseStrength(value); };
                    break;
                case BuffTypes.Agility:
                    action = (int value) => { characterParams.IncreaseAgility(value); };
                    break;
                case BuffTypes.Stamina:
                    action = (int value) => { characterParams.IncreaseStamina(value); };
                    break;
                case BuffTypes.Intelligence:
                    action = (int value) => { characterParams.IncreaseIntelligence(value); };
                    break;
                case BuffTypes.PhysicalDamage:
                    action = (int value) => { characterParams.IncreasePhysicalDamage(value); };
                    break;
                case BuffTypes.MagicDamage:
                    action = (int value) => { characterParams.IncreaseMagicalDamage(value); };
                    break;
                default:
                    break;
            }
            characterCombatManager.SetBuff(
                _buffValue,
                _roundsCount,
                _effectIcon,
                "",
                action,
                _inPercents
            );
        }

        public override void AddEffect(CardModifier cardModifier)
        {
            if (cardModifier.InPercents)
            {
                _buffValue += _buffValue * cardModifier.Value / 100;
                return;
            }
            _buffValue += cardModifier.Value;
        }
    }
}
