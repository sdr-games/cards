using System;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;
using SDRGames.Whist.CharacterCombatModule.Models;

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
            int damage = _damageValue;
            int roundsCount = _roundsCount;
            bool isCritical = false;
            string description = GetLocalizedDescription(casterParams);

            switch (_damageType)
            {
                case DamageTypes.Physical:
                    hitChance = UnityEngine.Random.Range(0, _chance + casterParams.PhysicalHitChance + casterParams.OnslaughtChance);
                    dodgeChance = UnityEngine.Random.Range(0, targetParams.DodgeChance + targetParams.BlockChance);
                    if (hitChance < dodgeChance)
                    {
                        damage = 0;
                        roundsCount = 0;
                        break;
                    }
                    isCritical = UnityEngine.Random.Range(0, 100) < casterParams.CriticalStrikeChance;

                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        damage = CalculatePercentageOfParameter(targetParams.ArmorPoints, damage);
                    }
                    else
                    {
                        damage += CalculatePhysicalDamage(casterParams);
                        damage = ApplyModifiers(casterParams, targetParams, damage, isCritical);
                    }

                    damage += damage + (int)Math.Round((decimal)damage / 100 * casterParams.PhysicalDamageModifier, MidpointRounding.ToEven);
                    damage -= CalculatePercentageOfParameter(targetParams.PhysicalDamageBlockPercent, damage);
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
                    if (hitChance < dodgeChance)
                    {
                        damage = 0;
                        roundsCount = 0;
                        break;
                    }
                    isCritical = UnityEngine.Random.Range(0, 100) < casterParams.CriticalStrikeChance;

                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        damage = CalculatePercentageOfParameter(targetParams.BarrierPoints, damage);
                    }
                    else
                    {
                        damage += CalculateMagicalDamage(casterParams);
                        damage = ApplyModifiers(casterParams, targetParams, damage, isCritical);
                    }

                    damage += damage + (int)Math.Round((decimal)damage / 100 * casterParams.MagicalDamageModifier, MidpointRounding.ToEven);
                    damage -= CalculatePercentageOfParameter(targetParams.MagicalDamageBlockPercent, damage);
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
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        damage = CalculatePercentageOfParameter(targetParams.HealthPoints, damage);
                    }
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
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        damage = CalculatePercentageOfParameter(((PlayerParamsModel)targetParams).PatientHealthPoints, damage);
                    }

                    damage -= CalculatePercentageOfParameter(((PlayerParamsModel)targetParams).PatientDamageBlockPercent, damage);
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
                targetCharacterCombatManager.SetPeriodicalChanges(damage, roundsCount, description, _effectIcon, action);
                return;
            }
            action(damage);
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

        public override string GetLocalizedDescription(CharacterParamsModel casterParams)
        {
            int damageValue = CalculateValue(casterParams);
            return GetLocalizedDescription(damageValue);
        }

        protected override string GetLocalizedDescription(int damageValue)
        {
            if (_inMaxPercents || _inCurrentPercents)
            {
                damageValue = _damageValue;
            }
            _description.SetParam("damage", damageValue);
            _description.SetParam("turns", _roundsCount);
            return _description.GetLocalizedText();
        }

        protected override int CalculateValue(CharacterParamsModel casterParams)
        {
            int result = _damageValue;
            switch (_damageType)
            {
                case DamageTypes.Physical:
                    result += CalculatePhysicalDamage(casterParams);
                    break;
                case DamageTypes.Magical:
                    result += CalculateMagicalDamage(casterParams);
                    break;
                default:
                    break;
            }
            return result;
        }

        private int CalculatePhysicalDamage(CharacterParamsModel casterParams)
        {
            double calculatedDamage = casterParams.Strength * CharacterParametersScaling.Instance.StrengthToPhysicalDamage;
            return (int)calculatedDamage;
        }

        private int CalculateMagicalDamage(CharacterParamsModel casterParams)
        {
            double calculatedDamage = casterParams.Intelligence * CharacterParametersScaling.Instance.IntelligenceToMagicalDamage;
            return (int)calculatedDamage;
        }

        private int ApplyModifiers(CharacterParamsModel casterParams, CharacterParamsModel targetParams, float calculatedDamage, bool isCritical)
        {
            bool isResilient = UnityEngine.Random.Range(0, 100) < targetParams.ResilienceChance;
            
            if (isCritical)
            {
                calculatedDamage *= CharacterParametersScaling.Instance.CriticalStrikeModifier;
            }
            if (isResilient)
            {
                calculatedDamage *= (100 - CharacterParametersScaling.Instance.ResiliencePercent) / 100f;
            }
            calculatedDamage *= 1 + targetParams.Weakening / 100f;
            calculatedDamage *= 1 - targetParams.Amplification / 100f;
            calculatedDamage *= 1 + casterParams.Amplification / 100f;

            return (int)Math.Round(calculatedDamage, MidpointRounding.ToEven);
        }
    }
}
