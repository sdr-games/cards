using System;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

using static SDRGames.Whist.AbilitiesModule.ScriptableObjects.RestorationLogicScriptableObject;

namespace SDRGames.Whist.AbilitiesModule.Models
{
    public class RestorationLogic : AbilityLogic
    {
        [SerializeField] private RestorationTypes _restorationType;

        [SerializeField] private int _restorationValue;

        public RestorationLogic(RestorationLogicScriptableObject restorationLogicScriptableObject) : base(restorationLogicScriptableObject)
        {
            _restorationType = restorationLogicScriptableObject.RestorationType;
            _restorationValue = restorationLogicScriptableObject.RestorationValue;
        }

        public override void Apply(CharacterCombatManager characterCombatManager)
        {
            int randomInt = UnityEngine.Random.Range(0, 100);
            if (_chance < randomInt)
            {
                return;
            }
            Action<int> action = null;

            switch (_restorationType)
            {
                case RestorationTypes.Armor:
                    action = (int value) => characterCombatManager.RestoreArmorPoints(value);
                    break;
                case RestorationTypes.Barrier:
                    action = (int value) => characterCombatManager.RestoreBarrierPoints(value);
                    break;
                case RestorationTypes.Health:
                    action = (int value) => characterCombatManager.RestoreHealthPoints(value);
                    break;
                case RestorationTypes.Stamina:
                    action = (int value) => characterCombatManager.RestoreStaminaPoints(value);
                    break;
                case RestorationTypes.Breath:
                    action = (int value) => characterCombatManager.RestoreBreathPoints(value);
                    break;
                default:
                    break;
            }
            if (_roundsCount > 1)
            {
                characterCombatManager.SetPeriodicalChanges(_restorationValue, _roundsCount, "", _effectIcon, action);
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
