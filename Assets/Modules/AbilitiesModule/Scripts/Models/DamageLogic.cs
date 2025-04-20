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
        private bool _inPercents;

        public DamageLogic(DamageLogicScriptableObject damageLogicScriptableObject) : base(damageLogicScriptableObject)
        {
            _damageType = damageLogicScriptableObject.DamageType;
            _damageValue = damageLogicScriptableObject.DamageValue;
            _inPercents = damageLogicScriptableObject.InPercents;
        }

        public override void Apply(CharacterCombatManager casterCombatManager)
        {
            throw new NotImplementedException();
        }

        public override void Apply(CharacterCombatManager casterCharacterCombatManager, CharacterCombatManager targetCharacterCombatManager)
        {
            CharacterParamsModel casterParams = casterCharacterCombatManager.GetParams();
            CharacterParamsModel targetParams = targetCharacterCombatManager.GetParams();
            Action<int> action = null;
            int hitChance = 0;
            int dodgeChance = 0;
            bool isCritical = false;

            switch (_damageType)
            {
                case DamageTypes.Physical:
                    hitChance = UnityEngine.Random.Range(0, _chance + casterParams.PhysicalHitChance + casterParams.OnslaughtChance);
                    Debug.Log($"hitchance {hitChance}");
                    dodgeChance = UnityEngine.Random.Range(0, targetParams.DodgeChance + targetParams.BlockChance);
                    Debug.Log($"dodgeChance {dodgeChance}");
                    if (hitChance < dodgeChance)
                    {
                        return;
                    }
                    isCritical = UnityEngine.Random.Range(0, 100) < casterParams.CriticalStrikeChance;
                    Debug.Log($"isCritical {isCritical}");
                    Debug.Log($"baseDamage {_damageValue}");

                    _damageValue += CalculatePhysicalDamage(casterParams, targetParams, isCritical);
                    _damageValue = ApplyModifiers(casterParams, targetParams, _damageValue, isCritical);
                    Debug.Log($"totalDamage {_damageValue}");
                    action = (int value) => targetCharacterCombatManager.TakePhysicalDamage(value, isCritical);
                    break;
                case DamageTypes.Magical:
                    hitChance = UnityEngine.Random.Range(0, _chance + casterParams.MagicalHitChance);
                    Debug.Log($"hitchance {hitChance}");
                    dodgeChance = UnityEngine.Random.Range(0, targetParams.DodgeChance);
                    Debug.Log($"dodgeChance {dodgeChance}");
                    if (hitChance < dodgeChance)
                    {
                        return;
                    }
                    isCritical = UnityEngine.Random.Range(0, 100) < casterParams.CriticalStrikeChance;
                    Debug.Log($"isCritical {isCritical}");
                    Debug.Log($"baseDamage {_damageValue}");

                    _damageValue += CalculateMagicalDamage(casterParams, targetParams, isCritical);
                    _damageValue = ApplyModifiers(casterParams, targetParams, _damageValue, isCritical);
                    Debug.Log($"totalDamage {_damageValue}");
                    action = (int value) => targetCharacterCombatManager.TakeMagicalDamage(value, isCritical);
                    break;
                case DamageTypes.True:
                    action = (int value) => targetCharacterCombatManager.TakeTrueDamage(value);
                    break;
                case DamageTypes.TruePatient:
                    action = (int value) => ((PlayerCombatManager)targetCharacterCombatManager).TakePatientDamage(value);
                    break;
                default:
                    break;
            }
            if (_roundsCount > 1)
            {
                targetCharacterCombatManager.SetPeriodicalChanges(_damageValue, _roundsCount, "", _effectIcon, action);
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

        private int CalculatePhysicalDamage(CharacterParamsModel casterParams, CharacterParamsModel targetParams, bool isCritical)
        {
            float calculatedDamage = casterParams.Strength * CharacterParametersScaling.Instance.StrengthToPhysicalDamage;
            Debug.Log($"calculatedDamage {calculatedDamage}");
            calculatedDamage += calculatedDamage * casterParams.PhysicalDamageModifier;
            Debug.Log($"calculatedDamageModified {calculatedDamage}");
            return (int)calculatedDamage;
        }

        private int CalculateMagicalDamage(CharacterParamsModel casterParams, CharacterParamsModel targetParams, bool isCritical)
        {
            float calculatedDamage = casterParams.Intelligence * CharacterParametersScaling.Instance.IntelligenceToMagicalDamage;
            calculatedDamage += calculatedDamage * casterParams.MagicalDamageModifier;
            return (int)calculatedDamage;
        }

        private int ApplyModifiers(CharacterParamsModel casterParams, CharacterParamsModel targetParams, float calculatedDamage, bool isCritical)
        {
            bool isResilient = UnityEngine.Random.Range(0, 100) < targetParams.ResilienceChance;

            if (isCritical)
            {
                calculatedDamage *= CharacterParametersScaling.Instance.CriticalStrikeModifier;
                Debug.Log($"calculatedDamageCritical {calculatedDamage}");
            }
            if (isResilient)
            {
                calculatedDamage *= 100 - CharacterParametersScaling.Instance.ResiliencePercent / 100f;
                Debug.Log($"calculatedDamageResilient {calculatedDamage}");
            }
            calculatedDamage *= 1 + targetParams.Weakening / 100f;
            Debug.Log($"calculatedDamageWeaked {calculatedDamage}");
            calculatedDamage *= 1 - targetParams.Amplification / 100f;
            Debug.Log($"calculatedDamageAmplified {calculatedDamage}");
            calculatedDamage *= 1 + casterParams.Amplification / 100f;
            Debug.Log($"calculatedDamageTotal {calculatedDamage}");

            return (int)Math.Round(calculatedDamage);
        }
    }
}
