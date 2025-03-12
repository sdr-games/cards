using System.Collections;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.EnemyBehaviorModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.EnemyBehaviorModule.Managers
{
    public class EnemyBehaviorManager : MonoBehaviour
    {
        private const int MAX_MELEE_ABILITIES_COUNT_PER_ROUND = 3;
        private const int MAX_MAGICAL_ABILITIES_COUNT_PER_ROUND = 3;

        [SerializeField] private BehaviorScriptableObject[] _meleeBehaviors;
        [SerializeField] private BehaviorScriptableObject[] _magicBehaviors;

        [field: SerializeField] public EnemyCombatManager EnemyCombatManager { get; private set; }

        private enum PreferrableDamageTypes { Physical, Magical }

        private PlayerCombatManager _playerCombatManager;
        private CharacterParamsModel _enemyParams;
        private CharacterParamsModel _playerParams;

        public void Initialize(PlayerCombatManager playerCombatManager, UserInputController userInputController)
        {
            _playerCombatManager = playerCombatManager;
            EnemyCombatManager.Initialize(userInputController);
            _enemyParams = EnemyCombatManager.GetParams();
            _playerParams = _playerCombatManager.GetParams();
            InitializeBehaviors(_meleeBehaviors);
            InitializeBehaviors(_magicBehaviors);
        }

        public virtual void MakeMove()
        {
            List<AbilityScriptableObject> selectedAbilities;
            int count = 0;
            if (DefinePreferrableDamageType() == PreferrableDamageTypes.Physical)
            {
                selectedAbilities = SelectAbilities(
                    _meleeBehaviors,
                    EnemyCombatManager.GetParams().StaminaPoints.CurrentValue,
                    _playerCombatManager.GetParams().ArmorPoints.CurrentValueInPercents
                );
                if(selectedAbilities != null)
                {
                    StartCoroutine(ApplySelectedAbilities(selectedAbilities));
                    EnemyCombatManager.SpendStaminaPoints(selectedAbilities.Sum(ability => ability.Cost));
                    count = selectedAbilities.Count;
                }
                EnemyCombatManager.RestoreStaminaPoints((MAX_MELEE_ABILITIES_COUNT_PER_ROUND - count) * _enemyParams.StaminaPoints.RestorationPower);
                return;
            }
            selectedAbilities = SelectAbilities(
                _magicBehaviors,
                EnemyCombatManager.GetParams().BreathPoints.CurrentValue,
                _playerCombatManager.GetParams().BarrierPoints.CurrentValueInPercents
            );
            if (selectedAbilities != null)
            {
                StartCoroutine(ApplySelectedAbilities(selectedAbilities));
                EnemyCombatManager.SpendBreathPoints(selectedAbilities.Sum(ability => ability.Cost));
                count = selectedAbilities.Count;
            }
            EnemyCombatManager.RestoreBreathPoints((MAX_MAGICAL_ABILITIES_COUNT_PER_ROUND + 1 - count) * _enemyParams.StaminaPoints.RestorationPower);
        }

        private PreferrableDamageTypes DefinePreferrableDamageType()
        {
            List<AbilityScriptableObject> availableMeleeAbilities = GetAllAbilties(_meleeBehaviors);
            float physicalAverageDamage = availableMeleeAbilities.Sum(ability => ability.GetAverageDamage());
            int totalMeleeAbilitiesCost = availableMeleeAbilities.Sum(ability => ability.Cost);

            float meleeAbilitiesCountWithoutRestoration = _enemyParams.StaminaPoints.MaxValue / totalMeleeAbilitiesCost;
            float maxMeleeRoundsWithoutRestoration = _enemyParams.StaminaPoints.MaxValue / totalMeleeAbilitiesCost / (MAX_MELEE_ABILITIES_COUNT_PER_ROUND - 1);
            float meleeAbilitiesCountWithRestoration = meleeAbilitiesCountWithoutRestoration + (maxMeleeRoundsWithoutRestoration * _enemyParams.StaminaPoints.RestorationPower / totalMeleeAbilitiesCost);
            float totalPhysicalDamage = physicalAverageDamage * meleeAbilitiesCountWithRestoration;
            float totalPhysicalDifference = _playerParams.ArmorPoints.CurrentValue + _playerParams.HealthPoints.CurrentValue - totalPhysicalDamage;

            List<AbilityScriptableObject> availableMagicAbilities = GetAllAbilties(_magicBehaviors);
            float magicalAverageDamage = availableMagicAbilities.Sum(ability => ability.GetAverageDamage());
            int totalMagicalAbilitiesCost = availableMagicAbilities.Sum(ability => ability.Cost);

            float magicalAbilitiesCountWithoutRestoration = _enemyParams.BreathPoints.MaxValue / totalMagicalAbilitiesCost;
            float maxMagicalRoundsWithoutRestoration = _enemyParams.BreathPoints.MaxValue / totalMagicalAbilitiesCost / (MAX_MAGICAL_ABILITIES_COUNT_PER_ROUND - 1);
            float magicalAbilitiesCountWithRestoration = magicalAbilitiesCountWithoutRestoration + (maxMagicalRoundsWithoutRestoration * _enemyParams.BreathPoints.RestorationPower / totalMagicalAbilitiesCost);
            float totalMagicalDamage = magicalAverageDamage * magicalAbilitiesCountWithRestoration;
            float totalMagicalDifference = _playerParams.BarrierPoints.CurrentValue + _playerParams.HealthPoints.CurrentValue - totalMagicalDamage;

            return totalPhysicalDifference < totalMagicalDifference ? PreferrableDamageTypes.Physical : PreferrableDamageTypes.Magical;
        }

        public List<AbilityScriptableObject> SelectAbilities(BehaviorScriptableObject[] behaviors, float currentResourceValue, float currentPlayerDefencePercents)
        {
            if (currentResourceValue <= 0)
            {
                return null;
            }

            List<AbilityScriptableObject> abilities = new List<AbilityScriptableObject>();
            foreach (BehaviorScriptableObject behavior in behaviors)
            {
                if (behavior.CheckIfAppliableByDefence(currentPlayerDefencePercents))
                {
                    abilities = behavior.ChooseRandomAttacks(currentResourceValue);
                    if(abilities is null && behavior.MinimalCost <= currentResourceValue)
                    {
                        abilities = behavior.MinimalCostAttacks;
                    }
                    break;
                }
            }

            if (abilities is null)
            {
                abilities = behaviors.OrderBy(behavior => behavior.MinimalCost).FirstOrDefault().MinimalCostAttacks;
            }
            return abilities;
        }

        public List<AbilityScriptableObject> GetAllAbilties(BehaviorScriptableObject[] behaviors)
        {
            List<AbilityScriptableObject> abilities = new List<AbilityScriptableObject>();
            foreach (BehaviorScriptableObject behavior in behaviors)
            {
                foreach (AbilityScriptableObject abilityScriptableObject in behavior.GetAllAbilities())
                {
                    if (abilities.Contains(abilityScriptableObject))
                    {
                        continue;
                    }
                    abilities.Add(abilityScriptableObject);
                }
            }
            return abilities;
        }

        private void InitializeBehaviors(BehaviorScriptableObject[] _behaviors)
        {
            foreach(BehaviorScriptableObject behavior in _behaviors)
            {
                behavior.Initialize();
            }
        }

        private IEnumerator ApplySelectedAbilities(List<AbilityScriptableObject> selectedAbilities)
        {
            yield return null;
            foreach (AbilityScriptableObject abilitySO in selectedAbilities)
            {
                EnemyCombatManager.SoundController.Play(abilitySO.SoundClip);
                new Ability(abilitySO).ApplyLogics(EnemyCombatManager, _playerCombatManager);
                yield return new WaitForSeconds(abilitySO.SoundClip.AudioClip.length);
            }
        }
    }
}
