using System;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;
using SDRGames.Whist.CharacterCombatModule.Models;

using UnityEngine;

using static SDRGames.Whist.AbilitiesModule.ScriptableObjects.DebuffLogicScriptableObject;

namespace SDRGames.Whist.AbilitiesModule.Models
{
    public class DebuffLogic : AbilityLogic
    {
        private DebuffTypes _debuffType;
        private int _debuffValue;

        public DebuffLogic(DebuffLogicScriptableObject debuffLogicScriptableObject) : base(debuffLogicScriptableObject)
        {
            _debuffType = debuffLogicScriptableObject.DebuffType;
            _debuffValue = debuffLogicScriptableObject.DebuffValue;
        }

        public override void Apply(CharacterCombatManager targetCharacterCombatManager, CharacterCombatManager casterCharacterCombatManager = null)
        {
            CharacterParamsModel targetParams = targetCharacterCombatManager.GetParams();
            if (_chance < UnityEngine.Random.Range(0, 100) + targetParams.DebuffBlockPercent)
            {
                return;
            }
            Action<int> action = null;
            int debuffValue = CalculateValue(targetParams);
            string description = GetLocalizedDescription(debuffValue);

            switch (_debuffType)
            {
                case DebuffTypes.Strength:
                    action = (int value) => { targetParams.ChangeStrength(value); };
                    break;
                case DebuffTypes.Agility:
                    action = (int value) => { targetParams.ChangeAgility(value); };
                    break;
                case DebuffTypes.Stamina:
                    action = (int value) => { targetParams.ChangeStamina(value); };
                    break;
                case DebuffTypes.Intelligence:
                    action = (int value) => { targetParams.ChangeIntelligence(value); };
                    break;
                case DebuffTypes.PhysicalDamage:
                    action = (int value) => { targetParams.ChangePhysicalDamage(value); };
                    break;
                case DebuffTypes.MagicalDamage:
                    action = (int value) => { targetParams.ChangeMagicalDamage(value); };
                    break;
                case DebuffTypes.PhysicalDamageBlock:
                    action = (int value) => { targetParams.SetPhysicalDamageBlockPercent(value); };
                    break;
                case DebuffTypes.MagicalDamageBlock:
                    action = (int value) => { targetParams.SetMagicalDamageBlockPercent(value); };
                    break;
                case DebuffTypes.PatientDamageBlock:
                    action = (int value) => { ((PlayerParamsModel)targetParams).SetPatientDamageBlockPercent(value); };
                    break;
                case DebuffTypes.Insanity:
                    action = (int value) => { ((EnemyCombatManager)targetCharacterCombatManager).BecomeInsane(value); };
                    break;
                case DebuffTypes.Advantage:
                    action = (int value) => { targetParams.SetAdvantagePercent(value); };
                    break;
                default:
                    break;
            }
            targetCharacterCombatManager.SetDebuff(
                -_debuffValue,
                _roundsCount,
                _effectIcon,
                description,
                action
            );
        }

        public override void AddEffect(AbilityModifier cardModifier)
        {
            if (cardModifier.InPercents)
            {
                _debuffValue += _debuffValue * cardModifier.Value / 100;
                return;
            }
            _debuffValue += cardModifier.Value;
        }

        public override string GetLocalizedDescription(CharacterParamsModel targetParams)
        {
            int debuffValue = CalculateValue(targetParams);
            return GetLocalizedDescription(debuffValue);
        }

        protected override string GetLocalizedDescription(int debuffValue)
        {
            if (_inMaxPercents || _inCurrentPercents)
            {
                debuffValue = _debuffValue;
            }
            _description.SetParam("debuff", debuffValue);
            _description.SetParam("turns", _roundsCount);
            return _description.GetLocalizedText();
        }

        protected override int CalculateValue(CharacterParamsModel targetParams)
        {
            int result = _debuffValue;
            if (_inMaxPercents || _inCurrentPercents)
            {
                switch (_debuffType)
                {
                    case DebuffTypes.Strength:
                        result = CalculatePercentageOfParameter(targetParams.Strength, result);
                        Debug.Log($"Процентное ослабление силы {result}");
                        break;
                    case DebuffTypes.Agility:
                        result = CalculatePercentageOfParameter(targetParams.Agility, result);
                        Debug.Log($"Процентное ослабление ловкости {result}");
                        break;
                    case DebuffTypes.Stamina:
                        result = CalculatePercentageOfParameter(targetParams.Stamina, result);
                        Debug.Log($"Процентное ослабление выносливости {result}");
                        break;
                    case DebuffTypes.Intelligence:
                        result = CalculatePercentageOfParameter(targetParams.Intelligence, result);
                        Debug.Log($"Процентное ослабление интеллекта {result}");
                        break;
                    case DebuffTypes.PhysicalDamage:
                        result = CalculatePercentageOfParameter(targetParams.PhysicalDamageModifier, result);
                        Debug.Log($"Процентное ослабление ПНФУ {result}");
                        break;
                    case DebuffTypes.MagicalDamage:
                        result = CalculatePercentageOfParameter(targetParams.MagicalDamageModifier, result);
                        Debug.Log($"Процентное ослабление ПНМУ {result}");
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
}
