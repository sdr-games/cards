using System;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.CharacterModule.Models;

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
            string description = GetLocalizedDescription();

            switch (_debuffType)
            {
                case DebuffTypes.Strength:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _debuffValue = CalculatePercentageOfParameter(targetParams.Strength, _debuffValue);
                        Debug.Log($"Процентное ослабление силы {_debuffValue}");
                    }

                    Debug.Log($"Финальное ослабление силы {_debuffValue}");
                    action = (int value) => { targetParams.ChangeStrength(value); };
                    break;
                case DebuffTypes.Agility:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _debuffValue = CalculatePercentageOfParameter(targetParams.Agility, _debuffValue);
                        Debug.Log($"Процентное ослабление ловкости {_debuffValue}");
                    }

                    Debug.Log($"Финальное ослабление ловкости {_debuffValue}");
                    action = (int value) => { targetParams.ChangeAgility(value); };
                    break;
                case DebuffTypes.Stamina:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _debuffValue = CalculatePercentageOfParameter(targetParams.Stamina, _debuffValue);
                        Debug.Log($"Процентное ослабление выносливости {_debuffValue}");
                    }

                    Debug.Log($"Финальное ослабление выносливости {_debuffValue}");
                    action = (int value) => { targetParams.ChangeStamina(value); };
                    break;
                case DebuffTypes.Intelligence:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _debuffValue = CalculatePercentageOfParameter(targetParams.Intelligence, _debuffValue);
                        Debug.Log($"Процентное ослабление интеллекта {_debuffValue}");
                    }

                    Debug.Log($"Финальное ослабление интеллекта {_debuffValue}");
                    action = (int value) => { targetParams.ChangeIntelligence(value); };
                    break;
                case DebuffTypes.PhysicalDamage:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _debuffValue = CalculatePercentageOfParameter(targetParams.PhysicalDamageModifier, _debuffValue);
                        Debug.Log($"Процентное ослабление ПНФУ {_debuffValue}");
                    }

                    Debug.Log($"Финальное ослабление ПНФУ {_debuffValue}");
                    action = (int value) => { targetParams.ChangePhysicalDamage(value); };
                    break;
                case DebuffTypes.MagicalDamage:
                    if (_inMaxPercents || _inCurrentPercents)
                    {
                        _debuffValue = CalculatePercentageOfParameter(targetParams.MagicalDamageModifier, _debuffValue);
                        Debug.Log($"Процентное ослабление ПНМУ {_debuffValue}");
                    }

                    Debug.Log($"Финальное ослабление ПНМУ {_debuffValue}");
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

        public override string GetLocalizedDescription()
        {
            return "";
        }
    }
}
