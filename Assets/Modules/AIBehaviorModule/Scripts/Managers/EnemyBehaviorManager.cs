using System.Collections;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.AIBehaviorModule.Managers
{
    public class EnemyBehaviorManager : MonoBehaviour
    {
        private const int MAX_MELEE_ABILITIES_COUNT_PER_ROUND = 3;
        private const int MAX_MAGICAL_ABILITIES_COUNT_PER_ROUND = 3;

        [SerializeField] private EnemyMeleeBehaviorManager _meleeBehaviorManager;

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
            DefinePreferrableDamageType();
        }

        public virtual void MakeMove()
        {
            if (DefinePreferrableDamageType() == PreferrableDamageTypes.Physical)
            {
                List<AbilityScriptableObject> selectedAbilities = _meleeBehaviorManager.SelectAbilities(
                    EnemyCombatManager.GetParams().StaminaPoints.CurrentValue,
                    _playerCombatManager.GetParams().ArmorPoints.CurrentValueInPercents
                );
                StartCoroutine(ApplySelectedAbilities(selectedAbilities));
                EnemyCombatManager.SpendStaminaPoints(selectedAbilities.Sum(ability => ability.Cost));
                EnemyCombatManager.RestoreStaminaPoints((MAX_MELEE_ABILITIES_COUNT_PER_ROUND - selectedAbilities.Count) * _enemyParams.StaminaPoints.RestorationPower);
            }
        }

        private PreferrableDamageTypes DefinePreferrableDamageType()
        {
            List<AbilityScriptableObject> availableMeleeAbilities = _meleeBehaviorManager.GetAllAbilties();
            float physicalAverageDamage = availableMeleeAbilities.Sum(ability => ability.GetAverageDamage());
            int totalMeleeAbilitiesCost = availableMeleeAbilities.Sum(ability => ability.Cost);

            float meleeAbilitiesCountWithoutRestoration = _enemyParams.StaminaPoints.MaxValue / totalMeleeAbilitiesCost;
            float maxMeleeRoundsWithoutRestoration = _enemyParams.StaminaPoints.MaxValue / totalMeleeAbilitiesCost / (MAX_MELEE_ABILITIES_COUNT_PER_ROUND - 1);
            float meleeAbilitiesCountWithRestoration = meleeAbilitiesCountWithoutRestoration + (maxMeleeRoundsWithoutRestoration * _enemyParams.StaminaPoints.RestorationPower / totalMeleeAbilitiesCost);
            float totalPhysicalDamage = physicalAverageDamage * meleeAbilitiesCountWithRestoration;
            float totalPhysicalDifference = _playerParams.ArmorPoints.CurrentValue + _playerParams.HealthPoints.CurrentValue - totalPhysicalDamage;

            List<AbilityScriptableObject> availableMagicAbilities = new List<AbilityScriptableObject>();
            float magicalAverageDamage = 1;
            int totalMagicalAbilitiesCost = 1;

            float magicalAbilitiesCountWithoutRestoration = _enemyParams.BreathPoints.MaxValue / totalMagicalAbilitiesCost;
            float maxMagicalRoundsWithoutRestoration = _enemyParams.BreathPoints.MaxValue / totalMagicalAbilitiesCost / (MAX_MAGICAL_ABILITIES_COUNT_PER_ROUND - 1);
            float magicalAbilitiesCountWithRestoration = magicalAbilitiesCountWithoutRestoration + (maxMagicalRoundsWithoutRestoration * _enemyParams.BreathPoints.RestorationPower / totalMagicalAbilitiesCost);
            float totalMagicalDamage = magicalAverageDamage * magicalAbilitiesCountWithRestoration;
            float totalMagicalDifference = _playerParams.BarrierPoints.CurrentValue + _playerParams.HealthPoints.CurrentValue - totalMagicalDamage;

            return totalPhysicalDifference < totalMagicalDifference ? PreferrableDamageTypes.Physical : PreferrableDamageTypes.Magical;
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
