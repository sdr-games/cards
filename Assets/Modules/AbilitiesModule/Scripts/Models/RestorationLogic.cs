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
                    action = (int value) => characterCombatManager.RestoreArmor(value);
                    break;
                case RestorationTypes.Barrier:
                    action = (int value) => characterCombatManager.RestoreBarrier(value);
                    break;
                case RestorationTypes.Health:
                    action = (int value) => characterCombatManager.RestoreHealth(value);
                    break;
                case RestorationTypes.Stamina:
                    action = (int value) => characterCombatManager.RestoreStamina(value);
                    break;
                case RestorationTypes.Breath:
                    action = (int value) => characterCombatManager.RestoreBreath(value);
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
    }
}
