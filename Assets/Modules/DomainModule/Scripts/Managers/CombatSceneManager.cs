using System;
using System.Collections.Generic;
using System.Collections;

using SDRGames.Whist.AbilitiesQueueModule.Managers;
using SDRGames.Whist.EnemyBehaviorModule.Managers;
using SDRGames.Whist.CardsCombatModule.Managers;
using SDRGames.Whist.CharacterCombatModule.Managers;
using SDRGames.Whist.MeleeCombatModule.Managers;
using SDRGames.Whist.TurnSwitchModule.Managers;
using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.CardsCombatModule.Models;

using UnityEngine;
using SDRGames.Whist.CharacterCombatModule.Models;
using SDRGames.Whist.ActiveBlockModule.Views;

namespace SDRGames.Whist.DomainModule.Managers
{
    public class CombatSceneManager : MonoBehaviour
    {
        private TurnsQueueManager _turnsQueueManager;

        private CombatUIManager _combatUIManager;
        private PlayerCombatManager _playerCombatManager;
        private List<EnemyBehaviorManager> _enemyBehaviorManagers;

        private List<EnemyCombatManager> _enemyCombatManagers;
        private List<CharacterCombatManager> _selectedEnemyCombatManagers;

        private int _currentEnemyIndex;
        private bool _targetCheckRequired;

        public void Initialize(TurnsQueueManager turnsQueueManager, CombatUIManager combatUIManager, PlayerCombatManager playerCombatManager, List<EnemyBehaviorManager> enemyBehaviorManagers, List<EnemyCombatManager> enemyCombatManagers)
        {
            _playerCombatManager = playerCombatManager;
            _playerCombatManager.PatientHealthChanged += OnPatientHealthChanged;

            _enemyBehaviorManagers = enemyBehaviorManagers;
            foreach (EnemyBehaviorManager enemyBehaviorManager in _enemyBehaviorManagers)
            {
                enemyBehaviorManager.AbilitiesSelected += OnEnemyAbiltiesSelected;
                enemyBehaviorManager.AbilityUsed += OnEnemyAbilityUsed;
                enemyBehaviorManager.BecameInsane += OnEnemyBecameInsane;
                enemyBehaviorManager.EnemyCombatManager.EnemySelected += OnEnemySelected;   
            }
            _enemyCombatManagers = enemyCombatManagers;

            _selectedEnemyCombatManagers = new List<CharacterCombatManager>();
            _targetCheckRequired = true;
            if(_enemyBehaviorManagers.Count <= 1)
            {
                _targetCheckRequired = false;
                _selectedEnemyCombatManagers.Add(_enemyCombatManagers[0]);
            }

            _combatUIManager = combatUIManager;
            _combatUIManager.AbilityQueueCleared += OnAbilityQueueCleared;
            _combatUIManager.CardSelectClicked += OnCardSelectClicked;
            _combatUIManager.CardMarkClicked += OnCardMarkClicked;
            _combatUIManager.MeleeAttackClicked += OnMeleeAttackClicked;
            _combatUIManager.StanceSwitched += OnStanceSwitched;
            _combatUIManager.EnemyAttacksBlockFinished += OnEnemyAttacksBlockFinished;
            _combatUIManager.CardsTurnEnd += OnCardsTurnEnd;
            _combatUIManager.MeleeTurnEnd += OnMeleeTurnEnd;
            _combatUIManager.ClearButtonClicked += OnClearButtonClicked;
            _combatUIManager.CardsSelectionCleared += OnCardsSelectionCleared;

            _turnsQueueManager = turnsQueueManager;
            _turnsQueueManager.TurnSwitched += OnTurnSwitched;
        }

        public void StartCombat()
        {
            _combatUIManager.Show();
            _turnsQueueManager.Run();
        }

        private void OnCardSelectClicked(object sender, CardSelectClickedEventArgs e)
        {
            if(!e.IsSelected && _playerCombatManager.HasEnoughBreathPoints(e.CardManager.Card.Cost))
            {
                if (_combatUIManager.TrySelectCard(e.CardManager))
                {
                    int cost = e.MarkedForDisenchant ? e.CardManager.Card.Cost * 2 : e.CardManager.Card.Cost; // multiplication by 2 required because first we have to revert reserve by card's mark and after then add reserve by card's selection
                    _playerCombatManager.ReserveBreathPoints(cost);
                }
                return;
            }
            if(_combatUIManager.TryDeselectCard(e.CardManager))
            {
                _playerCombatManager.ResetBreathReservedPoints(e.CardManager.Card.Cost);
            }
        }

        private void OnCardMarkClicked(object sender, CardMarkClickedEventArgs e)
        {
            if(!e.MarkedForDisenchant)
            {
                if (_combatUIManager.TryMarkCard(e.CardManager))
                {
                    int cost = e.IsSelected ? e.CardManager.Card.Cost * 2 : e.CardManager.Card.Cost; // multiplication by 2 required because first we have to revert reserve by card's selection and after then add reserve by card's mark
                    _playerCombatManager.ReserveBreathPoints(-cost);
                }
                return;
            }
            if(_combatUIManager.TryUnmarkCard(e.CardManager))
            {
                _playerCombatManager.ResetBreathReservedPoints(-e.CardManager.Card.Cost);
            }
        }

        private void OnMeleeAttackClicked(object sender, MeleeAttackClickedEventArgs e)
        {
            if(!_playerCombatManager.HasEnoughStaminaPoints(e.MeleeAttack.Cost))
            {
                return;
            }

            if (_combatUIManager.TryAddAbilityToQueue(e.MeleeAttack))
            {
                _playerCombatManager.ReserveStaminaPoints(e.MeleeAttack.Cost);
            }
        }

        private void OnStanceSwitched(object sender, StanceSwitchedEventArgs e)
        {
            float modifier = e.DefensiveStanceActive ? -0.25f : 0.25f;
            _playerCombatManager.SwitchStance(modifier);
        }

        private void OnEnemyAttacksBlockFinished(object sender, BlockKeyPressedEventArgs e)
        {
            StartCoroutine(FinishEnemyTurn(e.DamageMultiplier));
        }

        private void OnMeleeTurnEnd(object sender, MeleeEndTurnEventArgs e)
        {
            if (e.Abilities.Count > 0 && _selectedEnemyCombatManagers.Count == 0 && _targetCheckRequired)
            {
                _combatUIManager.ShowNoTargetError();
                return;
            }

            _combatUIManager.UnbindAbilitiesInQueue();
            _combatUIManager.HidePlayerUI();
            StartCoroutine(UseMeleeAbilities(e.Abilities));
        }

        private IEnumerator UseMeleeAbilities(List<Ability> abilities)
        {
            foreach (Ability ability in abilities)
            {
                yield return null;
                if (ability == null)
                {
                    _playerCombatManager.RestoreStaminaPoints();
                    continue;
                }

                while (!_playerCombatManager.AnimationsController.IsReady)
                {
                    yield return null;
                }
                _playerCombatManager.SpendStaminaPoints(ability.Cost);
                _playerCombatManager.AnimationsController.PlayAnimation(ability.AnimationClip);
                _playerCombatManager.SoundController.Play(ability.SoundClip);
                if (ability.AnimationClip != null)
                {
                    yield return new WaitForSeconds(ability.AnimationClip.averageDuration / 5);
                }
                ability.ApplyLogics(
                    _playerCombatManager,
                    _selectedEnemyCombatManagers
                );
                if (CheckVictoryConditions())
                {
                    EndBattle(true);
                    yield break;
                }
            }
            _turnsQueueManager.SwitchTurn();
        }

        private void OnCardsTurnEnd(object sender, CardsEndTurnEventArgs e)
        {
            if(e.SelectedCards.Count > 0 && _selectedEnemyCombatManagers.Count == 0 && _targetCheckRequired)
            {
                _combatUIManager.ShowNoTargetError();
                return;
            }

            _combatUIManager.RemoveUsedCards();
            _combatUIManager.HidePlayerUI();
            for (int i = 0; i < e.SelectedCards.Count; i++)
            {
                Card card = e.SelectedCards[i];
                if(card == null)
                {
                    continue;
                }

                if(i < e.SelectedCards.Count - i && card.AbilityComboScriptableObjects.Length > 0 && e.SelectedCards.Count > 1)
                {
                    List<Ability> affectedCards = new List<Ability>(e.SelectedCards);
                    for (int j = 0; j <= i; j++)
                    {
                        affectedCards.Remove(e.SelectedCards[j]);
                    }
                    card.ApplyCombo(_playerCombatManager, _selectedEnemyCombatManagers, affectedCards);
                    continue;
                }

                card.ApplyLogics(
                    _playerCombatManager,
                    _selectedEnemyCombatManagers
                );
                if(CheckVictoryConditions())
                {
                    EndBattle(true);
                    return;
                }
            }
            if (e.TotalCost > 0)
            {
                _playerCombatManager.SpendBreathPoints(e.TotalCost);
            }
            else
            {
                _playerCombatManager.RestoreBreathPoints(-e.TotalCost);
            }
            _turnsQueueManager.SwitchTurn();
        }

        private void OnAbilityQueueCleared(object sender, AbilityQueueClearedEventArgs e)
        {
            _playerCombatManager.ResetStaminaReservedPoints(e.ReverseAmount);
        }

        private void OnEnemySelected(object sender, MeshClickedEventArgs e)
        {
            EnemyCombatManager enemyCombatManager = (EnemyCombatManager)sender;
            if(e.IsSelected && !_selectedEnemyCombatManagers.Contains(enemyCombatManager))
            {
                _selectedEnemyCombatManagers.Add(enemyCombatManager);
            }
            else if(!e.IsSelected && _selectedEnemyCombatManagers.Contains(enemyCombatManager))
            {
                _selectedEnemyCombatManagers.Remove(enemyCombatManager);
            }
        }

        private void OnTurnSwitched(object sender, TurnSwitchedEventArgs e)
        {
            if(!e.IsCombatTurn)
            {
                _combatUIManager.ShowPlayerUI(false);
                return;
            }

            if (e.IsPlayerTurn)
            {
                StartPlayerTurn();
                return;
            }
            _currentEnemyIndex = e.EnemyIndex;
            StartCoroutine(StartEnemyTurn(_enemyBehaviorManagers[_currentEnemyIndex]));
        }

        private void OnClearButtonClicked(object sender, EventArgs e)
        {
            return;
        }

        private void OnCardsSelectionCleared(object sender, CardsSelectionClearedEventArgs e)
        {
            _playerCombatManager.ResetBreathReservedPoints(e.ReverseAmount);
        }

        private void OnPatientHealthChanged(object sender, PatientHealthChangedEventArgs e)
        {
            if (e.CurrentHealth <= 0)
            {
                _combatUIManager.ShowComaStartNotification();
                return;
            }
            _combatUIManager.ShowComaStopNotification();
        }

        private void OnEnemyAbiltiesSelected(object sender, AbilitiesSelectedEventArgs e)
        {
            if (e.ActiveBlockPossible)
            {
                _combatUIManager.ShowActiveBlockingPanel(0.5f);
                return;
            }
            StartCoroutine(FinishEnemyTurn(1));
        }

        private void OnEnemyAbilityUsed(object sender, EventArgs e)
        {
            if(CheckDefeatConditions())
            {
                ((EnemyBehaviorManager)sender).Stop();
                EndBattle(false);
            }
        }

        private void OnEnemyBecameInsane(object sender, EventArgs e)
        {
            ((EnemyBehaviorManager)sender).SetNewTarget(_enemyCombatManagers[UnityEngine.Random.Range(0, _enemyBehaviorManagers.Count - 1)]);
        }

        private void StartPlayerTurn()
        {
            _playerCombatManager.ApplyPeriodicalEffects();
            foreach (EnemyCombatManager enemyCharacterCombatManager in _enemyCombatManagers)
            {
                enemyCharacterCombatManager.UpdateBonusesEffects();
            }
            _combatUIManager.ShowPlayerUI(true);
        }

        private IEnumerator StartEnemyTurn(EnemyBehaviorManager enemyBehaviorManager)
        {
            _combatUIManager.HidePlayerUI();
            _playerCombatManager.UpdateBonusesEffects();
            enemyBehaviorManager.EnemyCombatManager.ApplyPeriodicalEffects();
            yield return enemyBehaviorManager.MakeMove();
        }

        private IEnumerator FinishEnemyTurn(float damageMultiplier = 1)
        {
            yield return _enemyBehaviorManagers[_currentEnemyIndex].ApplySelectedAbilities(damageMultiplier);
            _turnsQueueManager.SwitchTurn();
        }

        private bool CheckVictoryConditions()
        {
            int totalEnemiesHealth = 0;
            foreach (EnemyCombatManager enemyCombatManager in _enemyCombatManagers)
            {
                totalEnemiesHealth += (int)enemyCombatManager.GetParams().HealthPoints.CurrentValue;
            }
            return totalEnemiesHealth <= 0;
        }

        private bool CheckDefeatConditions()
        {
            PlayerParamsModel playerCharacterParamsModel = (PlayerParamsModel)_playerCombatManager.GetParams(); 
            return playerCharacterParamsModel.HealthPoints.CurrentValue <= 0 || (playerCharacterParamsModel.PatientHealthPoints.CurrentValue <= 0 && !_combatUIManager.PlayerHasAnyCardsOnHands());
        }

        private void EndBattle(bool victory)
        {
            _turnsQueueManager.Stop();
            if(victory)
            {
                _combatUIManager.ShowVictoryPanel();
                return;
            }
            _combatUIManager.ShowDefeatPanel();
        }

        private void OnDestroy()
        {
            _playerCombatManager.PatientHealthChanged -= OnPatientHealthChanged;
            foreach (EnemyBehaviorManager enemyBehaviorManager in _enemyBehaviorManagers)
            {
                enemyBehaviorManager.EnemyCombatManager.EnemySelected -= OnEnemySelected;
            }
            _combatUIManager.AbilityQueueCleared -= OnAbilityQueueCleared;
            _combatUIManager.CardSelectClicked -= OnCardSelectClicked;
            _combatUIManager.CardMarkClicked -= OnCardMarkClicked;
            _combatUIManager.MeleeAttackClicked -= OnMeleeAttackClicked;
            _combatUIManager.CardsTurnEnd -= OnCardsTurnEnd;
            _combatUIManager.MeleeTurnEnd -= OnMeleeTurnEnd;
            _combatUIManager.ClearButtonClicked -= OnClearButtonClicked;
            _turnsQueueManager.TurnSwitched -= OnTurnSwitched;
        }
    }
}
