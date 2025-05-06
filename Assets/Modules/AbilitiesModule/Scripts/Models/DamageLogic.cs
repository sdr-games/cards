using System;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.CharacterModule.Models;

using UnityEngine;

using static SDRGames.Whist.AbilitiesModule.ScriptableObjects.DamageLogicScriptableObject;

namespace SDRGames.Whist.AbilitiesModule.Models
{
    public class DamageLogic : AbilityLogic
    {
        private DamageTypes _damageType;
        private int _damageValue;

        public DamageLogic(DamageLogicScriptableObject damageLogicScriptableObject) : base(damageLogicScriptableObject)
        {
            _damageType = damageLogicScriptableObject.DamageType;
            _damageValue = damageLogicScriptableObject.DamageValue;
        }

        public override void Apply(CharacterCombatManager casterCharacterCombatManager, CharacterCombatManager targetCharacterCombatManager)
        {
            CharacterParamsModel casterParams = casterCharacterCombatManager.GetParams();
            CharacterParamsModel targetParams = targetCharacterCombatManager.GetParams();
            Action<int> action = null;
            int hitChance = 0;
            int dodgeChance = 0;
            bool isCritical = false;
            string description = GetLocalizedDescription();

            switch (_damageType)
            {
                case DamageTypes.Physical:
                    hitChance = UnityEngine.Random.Range(0, _chance + casterParams.PhysicalHitChance + casterParams.OnslaughtChance);
                    dodgeChance = UnityEngine.Random.Range(0, targetParams.DodgeChance + targetParams.BlockChance);
                    Debug.Log($"Шанс попадания: {hitChance} против шанса уклонения и блока: {dodgeChance}");
                    if (hitChance < dodgeChance)
                    {
                        _damageValue = 0;
                        _roundsCount = 0;
                        break;
                    }
                    isCritical = UnityEngine.Random.Range(0, 100) < casterParams.CriticalStrikeChance;
                    Debug.Log(isCritical ? $"Критическое попадание!" : $"Попадание");
                    Debug.Log($"Базовый урон {_damageValue}");

                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _damageValue = CalculatePercentageOfParameter(targetParams.ArmorPoints, _damageValue);
                        Debug.Log($"Процентный урон {_damageValue}");
                    }
                    else
                    {
                        _damageValue += CalculatePhysicalDamage(casterParams);
                        _damageValue = ApplyModifiers(casterParams, targetParams, _damageValue, isCritical);
                    }

                    _damageValue -= CalculatePercentageOfParameter(targetParams.PhysicalDamageBlockPercent, _damageValue);
                    Debug.Log($"Финальный расчетный физический урон {_damageValue}");
                    action = (int value) => { 
                        if(targetParams.ThornsPercent > 0)
                        {
                            int thornsDamage = CalculatePercentageOfParameter(targetParams.ThornsPercent, value);
                            value -= thornsDamage;
                            casterCharacterCombatManager.TakePhysicalDamage(thornsDamage, false);
                        }
                        targetCharacterCombatManager.TakePhysicalDamage(value, isCritical); 
                    };
                    break;
                case DamageTypes.Magical:
                    hitChance = UnityEngine.Random.Range(0, _chance + casterParams.MagicalHitChance);
                    dodgeChance = UnityEngine.Random.Range(0, targetParams.DodgeChance);
                    Debug.Log($"Шанс попадания: {hitChance} против шанса уклонения: {dodgeChance}");
                    if (hitChance < dodgeChance)
                    {
                        _damageValue = 0;
                        _roundsCount = 0;
                        break;
                    }
                    isCritical = UnityEngine.Random.Range(0, 100) < casterParams.CriticalStrikeChance;
                    Debug.Log(isCritical ? $"Критическое попадание!" : $"Попадание");
                    Debug.Log($"Базовый урон {_damageValue}");

                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _damageValue = CalculatePercentageOfParameter(targetParams.BarrierPoints, _damageValue);
                        Debug.Log($"Процентный урон {_damageValue}");
                    }
                    else
                    {
                        _damageValue += CalculateMagicalDamage(casterParams);
                        _damageValue = ApplyModifiers(casterParams, targetParams, _damageValue, isCritical);
                    }

                    _damageValue -= CalculatePercentageOfParameter(targetParams.MagicalDamageBlockPercent, _damageValue);
                    Debug.Log($"Финальный расчетный магический урон {_damageValue}");
                    action = (int value) =>
                    {
                        if (targetParams.ThornsPercent > 0)
                        {
                            int thornsDamage = CalculatePercentageOfParameter(targetParams.ThornsPercent, value);
                            value -= thornsDamage;
                            casterCharacterCombatManager.TakeMagicalDamage(thornsDamage, false);
                        }
                        targetCharacterCombatManager.TakeMagicalDamage(value, isCritical);
                    };
                    break;
                case DamageTypes.True:
                    Debug.Log($"Базовый урон {_damageValue}");
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _damageValue = CalculatePercentageOfParameter(targetParams.HealthPoints, _damageValue);
                        Debug.Log($"Процентный урон {_damageValue}");
                    }
                    Debug.Log($"Финальный расчетный прямой урон {_damageValue}");
                    action = (int value) =>
                    {
                        if (targetParams.ThornsPercent > 0)
                        {
                            int thornsDamage = CalculatePercentageOfParameter(targetParams.ThornsPercent, value);
                            value -= thornsDamage;
                            casterCharacterCombatManager.TakeTrueDamage(thornsDamage, false);
                        }
                        targetCharacterCombatManager.TakeTrueDamage(value);
                    };
                    break;
                case DamageTypes.TruePatient:
                    Debug.Log($"Базовый урон {_damageValue}");
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _damageValue = CalculatePercentageOfParameter(((PlayerParamsModel)targetParams).PatientHealthPoints, _damageValue);
                        Debug.Log($"Процентный урон {_damageValue}");
                    }

                    _damageValue -= CalculatePercentageOfParameter(((PlayerParamsModel)targetParams).PatientDamageBlockPercent, _damageValue);
                    Debug.Log($"Финальный расчетный урон по пациенту {_damageValue}");
                    action = (int value) =>
                    {
                        if (targetParams.ThornsPercent > 0)
                        {
                            int thornsDamage = CalculatePercentageOfParameter(targetParams.ThornsPercent, value);
                            value -= thornsDamage;
                            casterCharacterCombatManager.TakeTrueDamage(thornsDamage, false);
                        }
                        ((PlayerCombatManager)targetCharacterCombatManager).TakePatientDamage(value);
                    };
                    break;
                default:
                    break;
            }
            if (_roundsCount > 1)
            {
                targetCharacterCombatManager.SetPeriodicalChanges(_damageValue, _roundsCount, description, _effectIcon, action);
                return;
            }
            action(_damageValue);
        }

        public override void AddEffect(AbilityModifier cardModifier)
        {
            if (cardModifier.InPercents)
            {
                _damageValue += _damageValue * cardModifier.Value / 100;
                return;
            }
            _damageValue += cardModifier.Value;
        }

        public override string GetLocalizedDescription()
        {
            _description.SetParam("damage", _damageValue);
            return _description.GetLocalizedText();
        }

        private int CalculatePhysicalDamage(CharacterParamsModel casterParams)
        {
            float calculatedDamage = casterParams.Strength * CharacterParametersScaling.Instance.StrengthToPhysicalDamage;
            Debug.Log($"Урон с учетом Силы {calculatedDamage}");
            calculatedDamage += calculatedDamage * casterParams.PhysicalDamageModifier;
            Debug.Log($"Урон с учетом Показателя ФизУрона {calculatedDamage}");
            return (int)calculatedDamage;
        }

        private int CalculateMagicalDamage(CharacterParamsModel casterParams)
        {
            float calculatedDamage = casterParams.Intelligence * CharacterParametersScaling.Instance.IntelligenceToMagicalDamage;
            Debug.Log($"Урон с учетом Интеллекта {calculatedDamage}");
            calculatedDamage += calculatedDamage * casterParams.MagicalDamageModifier;
            Debug.Log($"Урон с учетом Показателя МагУрона {calculatedDamage}");
            return (int)calculatedDamage;
        }

        private int ApplyModifiers(CharacterParamsModel casterParams, CharacterParamsModel targetParams, float calculatedDamage, bool isCritical)
        {
            bool isResilient = UnityEngine.Random.Range(0, 100) < targetParams.ResilienceChance;

            if (isCritical)
            {
                calculatedDamage *= CharacterParametersScaling.Instance.CriticalStrikeModifier;
                Debug.Log($"Критический урон {calculatedDamage}");
            }
            if (isResilient)
            {
                calculatedDamage *= 100 - CharacterParametersScaling.Instance.ResiliencePercent / 100f;
                Debug.Log($"Урон с учетом устойчивости {calculatedDamage}");
            }
            calculatedDamage *= 1 + targetParams.Weakening / 100f;
            Debug.Log($"Урон с учетом ослабления цели {calculatedDamage}");
            calculatedDamage *= 1 - targetParams.Amplification / 100f;
            Debug.Log($"Урон с учетом усиления цели {calculatedDamage}");
            calculatedDamage *= 1 + casterParams.Amplification / 100f;
            Debug.Log($"Урон с учетом усиления {calculatedDamage}");

            return (int)Math.Round(calculatedDamage);
        }
    }
}
