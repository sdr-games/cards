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
            Action action = null;

            switch (_restorationType)
            {
                case RestorationType.Armor:
                    action = () => characterCombatManager.RestoreArmor(_restorationValue);
                    break;
                case RestorationType.Barrier:
                    action = () => characterCombatManager.RestoreBarrier(_restorationValue);
                    break;
                case RestorationType.Health:
                    action = () => characterCombatManager.RestoreHealth(_restorationValue);
                    break;
                case RestorationType.Stamina:
                    action = () => characterCombatManager.RestoreStamina(_restorationValue);
                    break;
                case RestorationType.Breath:
                    action = () => characterCombatManager.RestoreBreath(_restorationValue);
                    break;
                default:
                    break;
            }
            if (_roundsCount > 1)
            {
                characterCombatManager.SetPeriodicalChanges(_restorationValue, _roundsCount, EffectIcon, action);
                return;
            }
            action();
        }

        private void OnEnable()
        {
            SelfUsable = true;
        }
    }
}
