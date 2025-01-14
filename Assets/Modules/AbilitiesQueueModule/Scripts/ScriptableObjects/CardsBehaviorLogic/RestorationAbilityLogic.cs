using System;
using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "RestorationAbilityLogic", menuName = "SDRGames/Combat/Logics/Restoration Ability Logic")]
    public class RestorationAbilityLogic : AbilityLogicScriptableObject
    {
        private enum RestorationType { Armor, Barrier, Health, Stamina, Breath };
        [SerializeField] private RestorationType _restorationType;

        [SerializeField] private int _restorationValue;

        public override void Apply(CharacterCombatManager characterCombatManager)
        {
            int randomInt = UnityEngine.Random.Range(0, 100);
            if(_chance < randomInt)
            {
                return;
            }
            Action<int> action = null;

            switch (_restorationType)
            {
                case RestorationType.Armor:
                    action = (int value) => characterCombatManager.RestoreArmor(value);
                    break;
                case RestorationType.Barrier:
                    action = (int value) => characterCombatManager.RestoreBarrier(value);
                    break;
                case RestorationType.Health:
                    action = (int value) => characterCombatManager.RestoreHealth(value);
                    break;
                case RestorationType.Stamina:
                    action = (int value) => characterCombatManager.RestoreStamina(value);
                    break;
                case RestorationType.Breath:
                    action = (int value) => characterCombatManager.RestoreBreath(value);
                    break;
                default:
                    break;
            }
            if (_roundsCount > 1)
            {
                characterCombatManager.SetPeriodicalChanges(_restorationValue, _roundsCount, EffectIcon, action);
                return;
            }
            action(_restorationValue);
        }

        private void OnEnable()
        {
            SelfUsable = true;
        }
    }
}
