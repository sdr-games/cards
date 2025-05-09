using System;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;
using SDRGames.Whist.CharacterCombatModule.Models;

using UnityEngine;

using static SDRGames.Whist.AbilitiesModule.ScriptableObjects.BuffLogicScriptableObject;

namespace SDRGames.Whist.AbilitiesModule.Models
{
    public class BuffLogic : AbilityLogic
    {
        private BuffTypes _buffType;
        private int _buffValue;

        public BuffLogic(BuffLogicScriptableObject buffLogicScriptableObject) : base(buffLogicScriptableObject)
        {
            _buffType = buffLogicScriptableObject.BuffType;
            _buffValue = buffLogicScriptableObject.BuffValue;
        }

        public override void Apply(CharacterCombatManager targetCharacterCombatManager, CharacterCombatManager casterCharacterCombatManager = null)
        {
            CharacterParamsModel targetParams = targetCharacterCombatManager.GetParams();
            if (_chance < UnityEngine.Random.Range(0, 100))
            {
                return;
            }
            Action<int> action = null;
            int buffValue = CalculateValue(targetParams);
            string description = GetLocalizedDescription(buffValue);

            switch (_buffType)
            {
                case BuffTypes.HealthPoints:
                    action = (int value) => { targetParams.HealthPoints.IncreaseTemporaryBonus(value); };
                    break;
                case BuffTypes.Strength:
                    action = (int value) => { targetParams.ChangeStrength(value); };
                    break;
                case BuffTypes.Agility:
                    action = (int value) => { targetParams.ChangeAgility(value); };
                    break;
                case BuffTypes.Stamina:
                    action = (int value) => { targetParams.ChangeStamina(value); };
                    break;
                case BuffTypes.Intelligence:
                    action = (int value) => { targetParams.ChangeIntelligence(value); };
                    break;
                case BuffTypes.PhysicalDamage:
                    action = (int value) => { targetParams.ChangePhysicalDamage(value); };
                    break;
                case BuffTypes.MagicalDamage:
                    action = (int value) => { targetParams.ChangeMagicalDamage(value); };
                    break;
                case BuffTypes.PhysicalDamageBlock:
                    action = (int value) => { targetParams.SetPhysicalDamageBlockPercent(value); };
                    break;
                case BuffTypes.MagicalDamageBlock:
                    action = (int value) => { targetParams.SetMagicalDamageBlockPercent(value); };
                    break;
                case BuffTypes.PatientDamageBlock:
                    action = (int value) => { ((PlayerParamsModel)targetParams).SetPatientDamageBlockPercent(value); };
                    break;
                case BuffTypes.Sacrifice:
                    action = (int value) => { ((PlayerParamsModel)targetParams).SetSacrificePercent(value); };
                    break;
                case BuffTypes.Thorns:
                    action = (int value) => { targetParams.SetThornsPercent(value); };
                    break;
                case BuffTypes.Converting:
                    action = (int value) => { ((PlayerParamsModel)targetParams).SetConvertingPercent(value); };
                    break;
                case BuffTypes.DebuffsBlock:
                    action = (int value) => { targetParams.SetDebuffBlockPercent(value); };
                    break;
                case BuffTypes.ArmorPoints:
                    action = (int value) => { targetParams.ArmorPoints.IncreaseTemporaryBonus(value); };
                    break;
                case BuffTypes.BarrierPoints:
                    action = (int value) => { targetParams.BarrierPoints.IncreaseTemporaryBonus(value); };
                    break;
                case BuffTypes.Undying:
                    action = (int value) => { targetParams.HealthPoints.SetMinimalValue(value); };
                    break;
                case BuffTypes.UndyingPatient:
                    action = (int value) => { ((PlayerParamsModel)targetParams).PatientHealthPoints.SetMinimalValue(value); };
                    break;
                default:
                    break;
            }
            targetCharacterCombatManager.SetBuff(
                _buffValue,
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
                _buffValue += _buffValue * cardModifier.Value / 100;
                return;
            }
            _buffValue += cardModifier.Value;
        }

        public override string GetLocalizedDescription(CharacterParamsModel targetParams)
        {
            int buffValue = CalculateValue(targetParams);
            return GetLocalizedDescription(buffValue);
        }

        protected override string GetLocalizedDescription(int buffValue)
        {
            if(_inMaxPercents || _inCurrentPercents)
            {
                buffValue = _buffValue;
            }
            _description.SetParam("buff", buffValue);
            _description.SetParam("turns", _roundsCount);
            return _description.GetLocalizedText();
        }

        protected override int CalculateValue(CharacterParamsModel targetParams)
        {
            int result = _buffValue;
            if (_inMaxPercents || _inCurrentPercents)
            {
                switch (_buffType)
                {
                    case BuffTypes.HealthPoints:
                    case BuffTypes.Undying:
                        result = CalculatePercentageOfParameter(targetParams.HealthPoints, result);
                        Debug.Log($"Процентное усиление здоровья или порог бессмертия {result}");
                        break;
                    case BuffTypes.Strength:
                        result = CalculatePercentageOfParameter(targetParams.Strength, result);
                        Debug.Log($"Процентное усиление силы {result}");
                        break;
                    case BuffTypes.Agility:
                        result = CalculatePercentageOfParameter(targetParams.Agility, result);
                        Debug.Log($"Процентное усиление ловкости {result}");
                        break;
                    case BuffTypes.Stamina:
                        result = CalculatePercentageOfParameter(targetParams.Stamina, result);
                        Debug.Log($"Процентное усиление выносливости {result}");
                        break;
                    case BuffTypes.Intelligence:
                        result = CalculatePercentageOfParameter(targetParams.Intelligence, result);
                        Debug.Log($"Процентное усиление интеллекта {result}");
                        break;
                    case BuffTypes.PhysicalDamage:
                        result = CalculatePercentageOfParameter(targetParams.PhysicalDamageModifier, result);
                        Debug.Log($"Процентное усиление ПНФУ {result}");
                        break;
                    case BuffTypes.MagicalDamage:
                        result = CalculatePercentageOfParameter(targetParams.MagicalDamageModifier, result);
                        Debug.Log($"Процентное усиление ПНМУ {result}");
                        break;
                    case BuffTypes.ArmorPoints:
                        result = CalculatePercentageOfParameter(targetParams.ArmorPoints, result);
                        Debug.Log($"Процентное усиление брони {result}");
                        break;
                    case BuffTypes.BarrierPoints:
                        result = CalculatePercentageOfParameter(targetParams.BarrierPoints, result);
                        Debug.Log($"Процентное усиление барьера {result}");
                        break;
                    case BuffTypes.UndyingPatient:
                        result = CalculatePercentageOfParameter(((PlayerParamsModel)targetParams).PatientHealthPoints, result);
                        Debug.Log($"Процентный порог здоровья бессмертия пациента {result}");
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
}
