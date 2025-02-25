using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.AIBehaviorModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.AIBehaviorModule.Managers
{
    public class EnemyMeleeBehaviorManager : MonoBehaviour
    {
        [SerializeField] private BehaviorScriptableObject[] _behaviors;

        public List<AbilityScriptableObject> SelectAbilities(float currentResourceValue, float currentPlayerDefencePercents)
        {
            if (currentResourceValue <= 0)
            {
                return null;
            }

            List<AbilityScriptableObject> abilities = new List<AbilityScriptableObject>();
            BehaviorScriptableObject selectedBehavior = _behaviors.Where(
                behavior => behavior.CheckIfAppliableByArmor(currentPlayerDefencePercents) && behavior.CheckIfAppliableByResource(currentResourceValue)
            ).FirstOrDefault();

            if(selectedBehavior is null)
            {
                selectedBehavior = _behaviors.OrderBy(behavior => behavior.MinimalAttacksCost).FirstOrDefault();
                abilities = selectedBehavior.MinimalCostAttacks;
            }
            else
            {
                abilities = selectedBehavior.ChooseRandomAttacks();
            }
            return abilities;
        }

        public List<AbilityScriptableObject> GetAllAbilties()
        {
            List<AbilityScriptableObject> abilities = new List<AbilityScriptableObject>();
            foreach (BehaviorScriptableObject behavior in _behaviors)
            {
                foreach(AbilityScriptableObject abilityScriptableObject in behavior.GetAllAttacks())
                {
                    if(abilities.Contains(abilityScriptableObject))
                    {
                        continue;
                    }
                    abilities.Add(abilityScriptableObject);
                }
            }
            return abilities;
        }
    }
}
