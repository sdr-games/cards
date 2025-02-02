using System;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CardsCombatModule;
using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.CharacterModule.Managers;

using static SDRGames.Whist.AbilitiesModule.ScriptableObjects.DamageLogicScriptableObject;

namespace SDRGames.Whist.CardsCombatModule.Models
{
    public class DamageLogic : CardLogic
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

        public override void Apply(CharacterCombatManager targetCharacterCombatManager)
        {
            int randomInt = UnityEngine.Random.Range(0, 100);
            if (_chance < randomInt)
            {
                return;
            }
            Action<int> action = null;

            switch (_damageType)
            {
                case DamageTypes.Physical:
                    action = (int value) => targetCharacterCombatManager.TakePhysicalDamage(value);
                    break;
                case DamageTypes.Magical:
                    action = (int value) => targetCharacterCombatManager.TakeMagicalDamage(value);
                    break;
                case DamageTypes.True:
                    action = (int value) => targetCharacterCombatManager.TakeTrueDamage(value);
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

        public override void AddEffect(CardModifier cardModifier)
        {
            if (cardModifier.InPercents)
            {
                _damageValue += _damageValue * cardModifier.Value / 100;
                return;
            }
            _damageValue += cardModifier.Value;
        }
    }
}
