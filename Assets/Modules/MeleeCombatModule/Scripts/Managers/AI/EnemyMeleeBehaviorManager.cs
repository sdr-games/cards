using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.MeleeCombatModule.AI.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.MeleeCombatModule.AI.Managers
{
    public class EnemyMeleeBehaviorManager : MonoBehaviour
    {
        [SerializeField] private EnemyCharacterCombatManager _combatManager;
        [SerializeField] private PlayerCharacterCombatManager _playerCharacterCombatManager;
        [SerializeField] private BehaviorScriptableObject[] _behaviors;

        public void Initialize(EnemyCharacterCombatManager combatManager, PlayerCharacterCombatManager playerCharacterCombatManager)
        {
            _combatManager = combatManager;
            _playerCharacterCombatManager = playerCharacterCombatManager;
        }

        public void ChooseAndAppyAbilities()
        {
            if (_combatManager.GetParams().StaminaPoints.CurrentValue <= 0)
            {
                return;
            }

            List<AbilityScriptableObject> abilities = new List<AbilityScriptableObject>();
            BehaviorScriptableObject selectedBehavior = _behaviors.Where(
                behavior => behavior.CheckIfAppliableByArmor(_playerCharacterCombatManager.GetParams().ArmorPoints.CurrentValueInPercents)
            ).FirstOrDefault();

            if(selectedBehavior is null || !selectedBehavior.CheckIfAppliableByResource(_combatManager.GetParams().StaminaPoints.CurrentValue))
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
                ability.ApplyLogics(_combatManager, _playerCharacterCombatManager);
            }
            _combatManager.SpendStaminaPoints(abilities.Sum(ability => ability.Cost));
        }
    }
}
