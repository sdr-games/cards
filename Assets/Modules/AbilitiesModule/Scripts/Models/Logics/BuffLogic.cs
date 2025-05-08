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
            string description = GetLocalizedDescription();

            switch (_buffType)
            {
                case BuffTypes.HealthPoints:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _buffValue = CalculatePercentageOfParameter(targetParams.HealthPoints, _buffValue);
                        Debug.Log($"Процентное усиление здоровья {_buffValue}");
                    }

                    Debug.Log($"Финальное усиление здоровья {_buffValue}");
                    action = (int value) => { targetParams.HealthPoints.IncreaseTemporaryBonus(value); };
                    break;
                case BuffTypes.Strength:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _buffValue = CalculatePercentageOfParameter(targetParams.Strength, _buffValue);
                        Debug.Log($"Процентное усиление силы {_buffValue}");
                    }

                    Debug.Log($"Финальное усиление силы {_buffValue}");
                    action = (int value) => { targetParams.ChangeStrength(value); };
                    break;
                case BuffTypes.Agility:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _buffValue = CalculatePercentageOfParameter(targetParams.Agility, _buffValue);
                        Debug.Log($"Процентное усиление ловкости {_buffValue}");
                    }

                    Debug.Log($"Финальное усиление ловкости {_buffValue}");
                    action = (int value) => { targetParams.ChangeAgility(value); };
                    break;
                case BuffTypes.Stamina:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _buffValue = CalculatePercentageOfParameter(targetParams.Stamina, _buffValue);
                        Debug.Log($"Процентное усиление выносливости {_buffValue}");
                    }

                    Debug.Log($"Финальное усиление выносливости {_buffValue}");
                    action = (int value) => { targetParams.ChangeStamina(value); };
                    break;
                case BuffTypes.Intelligence:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _buffValue = CalculatePercentageOfParameter(targetParams.Intelligence, _buffValue);
                        Debug.Log($"Процентное усиление интеллекта {_buffValue}");
                    }

                    Debug.Log($"Финальное усиление интеллекта {_buffValue}");
                    action = (int value) => { targetParams.ChangeIntelligence(value); };
                    break;
                case BuffTypes.PhysicalDamage:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _buffValue = CalculatePercentageOfParameter(targetParams.PhysicalDamageModifier, _buffValue);
                        Debug.Log($"Процентное усиление ПНФУ {_buffValue}");
                    }

                    Debug.Log($"Финальное усиление ПНФУ {_buffValue}");
                    action = (int value) => { targetParams.ChangePhysicalDamage(value); };
                    break;
                case BuffTypes.MagicalDamage:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _buffValue = CalculatePercentageOfParameter(targetParams.MagicalDamageModifier, _buffValue);
                        Debug.Log($"Процентное усиление ПНМУ {_buffValue}");
                    }

                    Debug.Log($"Финальное усиление ПНМУ {_buffValue}");
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
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _buffValue = CalculatePercentageOfParameter(targetParams.ArmorPoints, _buffValue);
                        Debug.Log($"Процентное усиление брони {_buffValue}");
                    }

                    Debug.Log($"Финальное усиление брони {_buffValue}");
                    action = (int value) => { targetParams.ArmorPoints.IncreaseTemporaryBonus(value); };
                    break;
                case BuffTypes.BarrierPoints:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _buffValue = CalculatePercentageOfParameter(targetParams.BarrierPoints, _buffValue);
                        Debug.Log($"Процентное усиление барьера {_buffValue}");
                    }

                    Debug.Log($"Финальное усиление барьера {_buffValue}");
                    action = (int value) => { targetParams.BarrierPoints.IncreaseTemporaryBonus(value); };
                    break;
                case BuffTypes.Undying:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _buffValue = CalculatePercentageOfParameter(targetParams.HealthPoints, _buffValue);
                        Debug.Log($"Процентный порог здоровья бессмертия {_buffValue}");
                    }

                    Debug.Log($"Финальный порог здоровья бессмертия {_buffValue}");
                    action = (int value) => { targetParams.HealthPoints.SetMinimalValue(value); };
                    break;
                case BuffTypes.UndyingPatient:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _buffValue = CalculatePercentageOfParameter(((PlayerParamsModel)targetParams).PatientHealthPoints, _buffValue);
                        Debug.Log($"Процентный порог здоровья бессмертия пациента {_buffValue}");
                    }

                    Debug.Log($"Финальный порог здоровья бессмертия пациента {_buffValue}");
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

        public override string GetLocalizedDescription()
        {
            _description.SetParam("buff", _buffValue);
            return _description.GetLocalizedText();
        }
    }
}
