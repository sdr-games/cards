using System;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;
using SDRGames.Whist.CharacterCombatModule.Models;

using UnityEngine;

using static SDRGames.Whist.AbilitiesModule.ScriptableObjects.RestorationLogicScriptableObject;

namespace SDRGames.Whist.AbilitiesModule.Models
{
    public class RestorationLogic : AbilityLogic
    {
        private RestorationTypes _restorationType;
        private int _restorationValue;

        public RestorationLogic(RestorationLogicScriptableObject restorationLogicScriptableObject) : base(restorationLogicScriptableObject)
        {
            _restorationType = restorationLogicScriptableObject.RestorationType;
            _restorationValue = restorationLogicScriptableObject.RestorationValue;
        }

        public override void Apply(CharacterCombatManager targetCharacterCombatManager, CharacterCombatManager casterCharacterCombatManager = null)
        {
            CharacterParamsModel targetParams = targetCharacterCombatManager.GetParams();
            if (_chance < UnityEngine.Random.Range(0, 100))
            {
                return;
            }
            Action<int> action = null;
            int restorationValue = CalculateValue(targetParams);
            string description = GetLocalizedDescription(restorationValue);

            switch (_restorationType)
            {
                case RestorationTypes.Armor:
                    action = (int value) => targetCharacterCombatManager.RestoreArmorPoints(value);
                    break;
                case RestorationTypes.Barrier:
                    action = (int value) => targetCharacterCombatManager.RestoreBarrierPoints(value);
                    break;
                case RestorationTypes.Health:
                    action = (int value) => targetCharacterCombatManager.RestoreHealthPoints(value);
                    break;
                case RestorationTypes.Stamina:
                    action = (int value) => targetCharacterCombatManager.RestoreStaminaPoints(value);
                    break;
                case RestorationTypes.Breath:
                    action = (int value) => targetCharacterCombatManager.RestoreBreathPoints(value);
                    break;
                case RestorationTypes.PatientHealth:
                    action = (int value) => ((PlayerCombatManager)targetCharacterCombatManager).RestorePatientHealthPoints(value);
                    break;
                case RestorationTypes.Dispel:
                    action = (int value) => { targetCharacterCombatManager.ClearNegativeEffects(); };
                    break;
                case RestorationTypes.Swap:
                    action = (int value) => { ((PlayerCombatManager)targetCharacterCombatManager).Swap(); };
                    break;
                default:
                    break;
            }
            if (_roundsCount > 1)
            {
                targetCharacterCombatManager.SetPeriodicalChanges(restorationValue, _roundsCount, description, _effectIcon, action);
                return;
            }
            action(restorationValue);
        }

        public override void AddEffect(AbilityModifier cardModifier)
        {
            if (cardModifier.InPercents)
            {
                _restorationValue += _restorationValue * cardModifier.Value / 100;
                return;
            }
            _restorationValue += cardModifier.Value;
        }

        public override string GetLocalizedDescription(CharacterParamsModel targetParams)
        {
            int restorationValue = CalculateValue(targetParams);
            return GetLocalizedDescription(restorationValue);
        }

        protected override string GetLocalizedDescription(int restorationValue)
        {
            if (_inMaxPercents || _inCurrentPercents)
            {
                restorationValue = _restorationValue;
            }
            _description.SetParam("restore", restorationValue);
            _description.SetParam("turns", _roundsCount);
            return _description.GetLocalizedText();
        }

        protected override int CalculateValue(CharacterParamsModel targetParams)
        {
            int result = _restorationValue;
            if (_inMaxPercents || _inCurrentPercents)
            {
                switch (_restorationType)
                {
                    case RestorationTypes.Armor:
                        result = CalculatePercentageOfParameter(targetParams.ArmorPoints, result);
                        Debug.Log($"Процентное восстановление брони {result}");
                        break;
                    case RestorationTypes.Barrier:
                        result = CalculatePercentageOfParameter(targetParams.ArmorPoints, result);
                        Debug.Log($"Процентное восстановление барьера {result}");
                        break;
                    case RestorationTypes.Health:
                        result = CalculatePercentageOfParameter(targetParams.HealthPoints, result);
                        Debug.Log($"Процентное исцеление {result}");
                        break;
                    case RestorationTypes.Stamina:
                        result = CalculatePercentageOfParameter(targetParams.ArmorPoints, result);
                        Debug.Log($"Процентное восстановление выносливости {result}");
                        break;
                    case RestorationTypes.Breath:
                        result = CalculatePercentageOfParameter(targetParams.BarrierPoints, result);
                        Debug.Log($"Процентное восстановление дыхания {result}");
                        break;
                    case RestorationTypes.PatientHealth:
                        result = CalculatePercentageOfParameter(((PlayerParamsModel)targetParams).PatientHealthPoints, result);
                        Debug.Log($"Процентное исцеление здоровья пациента {result}");
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
}
