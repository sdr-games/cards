using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;
using SDRGames.Whist.AIBehaviorModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.AIBehaviorModule.Managers
{
    public class EnemyMeleeBehaviorManager : MonoBehaviour
    {
        [SerializeField] private EnemyCombatManager _combatManager;
        [SerializeField] private BehaviorScriptableObject[] _behaviors;

        private PlayerCombatManager _playerCombatManager;

        public void Initialize(EnemyCombatManager combatManager, PlayerCombatManager playerCombatManager)
        {
            _combatManager = combatManager;
            _playerCombatManager = playerCombatManager;
        }

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
            foreach (AbilityScriptableObject ability in abilities)
            {
                ability.ApplyLogics(_combatManager, _playerCombatManager, abilities.Count);
            }
            _combatManager.SpendStaminaPoints(abilities.Sum(ability => ability.Cost));
        }
    }
}
