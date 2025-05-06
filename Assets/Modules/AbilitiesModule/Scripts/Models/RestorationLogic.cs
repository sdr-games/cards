using System;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.CharacterModule.Models;

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
            string description = GetLocalizedDescription();

            switch (_restorationType)
            {
                case RestorationTypes.Armor:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _restorationValue = CalculatePercentageOfParameter(targetParams.ArmorPoints, _restorationValue);
                        Debug.Log($"Процентное восстановление брони {_restorationValue}");
                    }

                    Debug.Log($"Финальное восстановление брони {_restorationValue}");
                    action = (int value) => targetCharacterCombatManager.RestoreArmorPoints(value);
                    break;
                case RestorationTypes.Barrier:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _restorationValue = CalculatePercentageOfParameter(targetParams.ArmorPoints, _restorationValue);
                        Debug.Log($"Процентное восстановление барьера {_restorationValue}");
                    }

                    Debug.Log($"Финальное восстановление барьера {_restorationValue}");
                    action = (int value) => targetCharacterCombatManager.RestoreBarrierPoints(value);
                    break;
                case RestorationTypes.Health:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _restorationValue = CalculatePercentageOfParameter(targetParams.HealthPoints, _restorationValue);
                        Debug.Log($"Процентное исцеление {_restorationValue}");
                    }

                    Debug.Log($"Финальное исцеление {_restorationValue}");
                    action = (int value) => targetCharacterCombatManager.RestoreHealthPoints(value);
                    break;
                case RestorationTypes.Stamina:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _restorationValue = CalculatePercentageOfParameter(targetParams.ArmorPoints, _restorationValue);
                        Debug.Log($"Процентное восстановление выносливости {_restorationValue}");
                    }

                    Debug.Log($"Финальное восстановление выносливости {_restorationValue}");
                    action = (int value) => targetCharacterCombatManager.RestoreStaminaPoints(value);
                    break;
                case RestorationTypes.Breath:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _restorationValue = CalculatePercentageOfParameter(targetParams.BarrierPoints, _restorationValue);
                        Debug.Log($"Процентное восстановление дыхания {_restorationValue}");
                    }

                    Debug.Log($"Финальное восстановление дыхания {_restorationValue}");
                    action = (int value) => targetCharacterCombatManager.RestoreBreathPoints(value);
                    break;
                case RestorationTypes.PatientHealth:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _restorationValue = CalculatePercentageOfParameter(((PlayerParamsModel)targetParams).PatientHealthPoints, _restorationValue);
                        Debug.Log($"Процентное исцеление здоровья пациента {_restorationValue}");
                    }

                    Debug.Log($"Финальное исцеление здоровья пациента {_restorationValue}");
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
                targetCharacterCombatManager.SetPeriodicalChanges(_restorationValue, _roundsCount, description, _effectIcon, action);
                return;
            }
            action(_restorationValue);
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

        public override string GetLocalizedDescription()
        {
            _description.SetParam("restore", _restorationValue);
            return _description.GetLocalizedText();
        }
    }
}
