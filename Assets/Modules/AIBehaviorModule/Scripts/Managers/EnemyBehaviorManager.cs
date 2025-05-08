using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.EnemyBehaviorModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;
using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.CharacterCombatModule.Models;

using UnityEngine;
using SDRGames.Whist.CharacterCombatModule.ScriptableObjects;

namespace SDRGames.Whist.EnemyBehaviorModule.Managers
{
    public class EnemyBehaviorManager : MonoBehaviour
    {
        private const int MAX_MELEE_ABILITIES_COUNT_PER_ROUND = 3;
        private const int MAX_MAGICAL_ABILITIES_COUNT_PER_ROUND = 3;
        [field: SerializeField] public EnemyCombatManager EnemyCombatManager { get; private set; }
        private CharacterParamsModel _enemyParams;

        private CharacterCombatManager _targetCombatManager;
        private PlayerCombatManager _playerCombatManager;
        private CharacterParamsModel _targetParams;

        private List<SpecialAbility> _specialAbilities;
        private BehaviorScriptableObject[] _meleeBehaviors;
        private BehaviorScriptableObject[] _magicBehaviors;

        private int _insanityTurns;

        public event EventHandler AbilityUsed;
        public event EventHandler BecameInsane;

        public void Initialize(CharacterParamsScriptableObject enemyParamsScriptableObject, BehaviorScriptableObject[] meleeBehaviors, BehaviorScriptableObject[] magicBehaviors, SpecialAbilityScriptableObject[] specialAbilitiesScriptableObjects, GameObject modelPrefab, PlayerCombatManager playerCombatManager, UserInputController userInputController)
        {
            EnemyCombatManager.Initialize(enemyParamsScriptableObject, modelPrefab, userInputController);
            EnemyCombatManager.BecameInsane += OnBecameInsane;
            _enemyParams = EnemyCombatManager.GetParams();

            _playerCombatManager = playerCombatManager;
            _targetCombatManager = _playerCombatManager;
            _targetParams = _targetCombatManager.GetParams();

            _meleeBehaviors = meleeBehaviors;
            _magicBehaviors = magicBehaviors;

            _insanityTurns = 0;

            InitializeSpecialAbilities(specialAbilitiesScriptableObjects);
            InitializeBehavior(_meleeBehaviors);
            InitializeBehavior(_magicBehaviors);

            gameObject.SetActive(true);
        }

        public virtual void MakeMove()
        {
            if(UseSpecialAbility())
            {
                return;
            }

            if(_insanityTurns >= 0)
            {
                _insanityTurns--;
            }
            else
            {
                EndInsanity();
            }

            List<AbilityScriptableObject> selectedAbilities;
            int count = 0;
            if (PreferPhysicalDamage())
            {
                selectedAbilities = SelectAbilities(
                    _meleeBehaviors,
                    EnemyCombatManager.GetParams().StaminaPoints.CurrentValue,
                    _targetParams.ArmorPoints.CurrentValueInPercents
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
                _targetParams.BarrierPoints.CurrentValueInPercents
            );
            if (selectedAbilities != null)
            {
                StartCoroutine(ApplySelectedAbilities(selectedAbilities));
                EnemyCombatManager.SpendBreathPoints(selectedAbilities.Sum(ability => ability.Cost));
                count = selectedAbilities.Count;
            }
            EnemyCombatManager.RestoreBreathPoints((MAX_MAGICAL_ABILITIES_COUNT_PER_ROUND + 1 - count) * _enemyParams.BreathPoints.RestorationPower);
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

        public void Stop()
        {
            StopAllCoroutines();
        }

        public void OnBecameInsane(object sender, BecameInsaneEventArgs e)
        {
            _insanityTurns = e.InsanityTurns;
            BecameInsane?.Invoke(this, EventArgs.Empty);
        }

        public void SetNewTarget(CharacterCombatManager targetCombatManager)
        {
            _targetCombatManager = targetCombatManager;
            _targetParams = _targetCombatManager.GetParams();
        }

        public void EndInsanity()
        {
            _insanityTurns = 0;
            _targetCombatManager = _playerCombatManager;
            _targetParams = _targetCombatManager.GetParams();
        }

        private void InitializeBehavior(BehaviorScriptableObject[] behaviors)
        {
            foreach(BehaviorScriptableObject behavior in behaviors)
            {
                behavior.Initialize();
            }
        }

        private void InitializeSpecialAbilities(SpecialAbilityScriptableObject[] specialAbilitiesScriptableObjects)
        {
            _specialAbilities = new List<SpecialAbility>();
            foreach (SpecialAbilityScriptableObject specialAbilitySO in specialAbilitiesScriptableObjects)
            {
                SpecialAbility specialAbility = new SpecialAbility(specialAbilitySO);
                _specialAbilities.Add(specialAbility);
            }
        }

        private bool PreferPhysicalDamage()
        {
            if (_meleeBehaviors == null || _meleeBehaviors.Length == 0)
            {
                return false;
            }

            if (_magicBehaviors == null || _magicBehaviors.Length == 0)
            {
                return true;
            }

            List<AbilityScriptableObject> availableMeleeAbilities = GetAllAbilties(_meleeBehaviors);
            float physicalAverageDamage = availableMeleeAbilities.Sum(ability => ability.GetAverageDamage());
            int totalMeleeAbilitiesCost = availableMeleeAbilities.Sum(ability => ability.Cost);

            float meleeAbilitiesCountWithoutRestoration = _enemyParams.StaminaPoints.MaxValue / totalMeleeAbilitiesCost;
            float maxMeleeRoundsWithoutRestoration = _enemyParams.StaminaPoints.MaxValue / totalMeleeAbilitiesCost / (MAX_MELEE_ABILITIES_COUNT_PER_ROUND - 1);
            float meleeAbilitiesCountWithRestoration = meleeAbilitiesCountWithoutRestoration + (maxMeleeRoundsWithoutRestoration * _enemyParams.StaminaPoints.RestorationPower / totalMeleeAbilitiesCost);
            float totalPhysicalDamage = physicalAverageDamage * meleeAbilitiesCountWithRestoration;
            float totalPhysicalDifference = _targetParams.ArmorPoints.CurrentValue + _targetParams.HealthPoints.CurrentValue - totalPhysicalDamage;

            List<AbilityScriptableObject> availableMagicAbilities = GetAllAbilties(_magicBehaviors);
            float magicalAverageDamage = availableMagicAbilities.Sum(ability => ability.GetAverageDamage());
            int totalMagicalAbilitiesCost = availableMagicAbilities.Sum(ability => ability.Cost);

            float magicalAbilitiesCountWithoutRestoration = _enemyParams.BreathPoints.MaxValue / totalMagicalAbilitiesCost;
            float maxMagicalRoundsWithoutRestoration = _enemyParams.BreathPoints.MaxValue / totalMagicalAbilitiesCost / (MAX_MAGICAL_ABILITIES_COUNT_PER_ROUND - 1);
            float magicalAbilitiesCountWithRestoration = magicalAbilitiesCountWithoutRestoration + (maxMagicalRoundsWithoutRestoration * _enemyParams.BreathPoints.RestorationPower / totalMagicalAbilitiesCost);
            float totalMagicalDamage = magicalAverageDamage * magicalAbilitiesCountWithRestoration;
            float totalMagicalDifference = _targetParams.BarrierPoints.CurrentValue + _targetParams.HealthPoints.CurrentValue - totalMagicalDamage;

            return totalPhysicalDifference < totalMagicalDifference;
        }

        private bool UseSpecialAbility()
        {
            if(_insanityTurns > 0)
            {
                return false;
            }

            foreach(SpecialAbility specialAbility in _specialAbilities)
            {
                if (specialAbility.CurrentCooldown > 0)
                {
                    specialAbility.DecreaseCooldown();
                    continue;
                }
                StartCoroutine(ApplySelectedAbility(specialAbility));
                specialAbility.SetCooldown();
                return true;
            }
            return false;
        }

        private IEnumerator ApplySelectedAbilities(List<AbilityScriptableObject> selectedAbilities)
        {
            yield return null;
            foreach (AbilityScriptableObject abilitySO in selectedAbilities)
            {
                Ability ability = new Ability(abilitySO);
                yield return ApplySelectedAbility(ability);
            }
        }

        private IEnumerator ApplySelectedAbility(Ability ability)
        {
            EnemyCombatManager.SoundController.Play(ability.SoundClip);
            ability.ApplyLogics(EnemyCombatManager, _targetCombatManager);
            AbilityUsed?.Invoke(this, EventArgs.Empty);
            yield return new WaitForSeconds(ability.SoundClip.AudioClip.length);
        }
    }
}
