using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;
using SDRGames.Whist.AIMeleeCombatModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.AIMeleeCombatModule.Managers
{
    public class EnemyMeleeBehaviorManager : MonoBehaviour
    {
        [SerializeField] private BehaviorScriptableObject[] _behaviors;

        public void ChooseAndAppyAbilities(float currentResourceValue, float currentPlayerDefencePercents)
        {
            if (currentResourceValue <= 0)
            {
                return;
            }

            List<AbilityScriptableObject> abilities = new List<AbilityScriptableObject>();
            BehaviorScriptableObject selectedBehavior = _behaviors.Where(
                behavior => behavior.CheckIfAppliableByArmor(currentPlayerDefencePercents)
            ).FirstOrDefault();

            if(selectedBehavior is null || !selectedBehavior.CheckIfAppliableByResource(currentResourceValue))
            {
                selectedBehavior = _behaviors.OrderBy(behavior => behavior.MinimalAttacksCost).FirstOrDefault();
                abilities = selectedBehavior.MinimalCostAttacks;
            }
            else
            {
                abilities = selectedBehavior.ChooseRandomAttacks();
            }
            ApplyAbilities(abilities);
        }

        private void ApplyAbilities(List<AbilityScriptableObject> abilities)
        {
            //foreach (AbilityScriptableObject ability in abilities)
            //{
            //    ability.ApplyLogics(_combatManager, _playerCharacterCombatManager, abilities.Count);
            //}
            //_combatManager.SpendStaminaPoints(abilities.Sum(ability => ability.Cost));
        }
    }
}
