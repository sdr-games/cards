using System;
using System.Collections.Generic;
using System.Collections;

using SDRGames.Whist.AbilitiesQueueModule.Managers;
using SDRGames.Whist.EnemyBehaviorModule.Managers;
using SDRGames.Whist.CardsCombatModule.Managers;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.MeleeCombatModule.Managers;
using SDRGames.Whist.TurnSwitchModule.Managers;
using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.MusicModule.Managers;

using UnityEngine;
using SDRGames.Whist.MusicModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.ScriptableObjects;

namespace SDRGames.Whist.DomainModule.Managers
{
    public class CombatSceneManager : MonoBehaviour
    {
        private TurnsQueueManager _turnsQueueManager;

        private CombatUIManager _combatUIManager;
        private PlayerCombatManager _playerCombatManager;
        private EnemyBehaviorManager[] _enemyBehaviorManagers;

        private List<EnemyCombatManager> _enemyCombatManagers;
        private List<CharacterCombatManager> _selectedEnemyCombatManagers;
        private bool _targetCheckRequired;

        public void Initialize(TurnsQueueManager turnsQueueManager, CombatUIManager combatUIManager, PlayerCombatManager playerCombatManager, EnemyBehaviorManager[] enemyBehaviorManagers, List<EnemyCombatManager> enemyCombatManagers)
        {
            _playerCombatManager = playerCombatManager;
            _playerCombatManager.PatientHealthChanged += OnPatientHealthChanged;

            _enemyBehaviorManagers = enemyBehaviorManagers;
            foreach (EnemyBehaviorManager enemyBehaviorManager in _enemyBehaviorManagers)
            {
                enemyBehaviorManager.AbilityUsed += OnEnemyAbilityUsed;
                enemyBehaviorManager.EnemyCombatManager.EnemySelected += OnEnemySelected;   
            }
            _enemyCombatManagers = enemyCombatManagers;

            _selectedEnemyCombatManagers = new List<CharacterCombatManager>();
            _targetCheckRequired = true;
            if(_enemyBehaviorManagers.Length <= 1)
            {
                _targetCheckRequired = false;
                _selectedEnemyCombatManagers.Add(_enemyCombatManagers[0]);
            }

            _combatUIManager = combatUIManager;
            _combatUIManager.AbilityQueueCleared += OnAbilityQueueCleared;
            _combatUIManager.CardSelectClicked += OnCardSelectClicked;
            _combatUIManager.CardMarkClicked += OnCardMarkClicked;
            _combatUIManager.MeleeAttackClicked += OnMeleeAttackClicked;
            _combatUIManager.CardsTurnEnd += OnCardsTurnEnd;
            _combatUIManager.MeleeTurnEnd += OnMeleeTurnEnd;
            _combatUIManager.ClearButtonClicked += OnClearButtonClicked;
            _combatUIManager.CardsSelectionCleared += OnCardsSelectionCleared;

            _turnsQueueManager = turnsQueueManager;
            _turnsQueueManager.TurnSwitched += OnTurnSwitched;
        }

        public void StartCombat()
        {
            _combatUIManager.gameObject.SetActive(true);
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

                if(i < e.SelectedCards.Count && card.CardModifiersScriptableObjects.Length > 0)
                {
                    List<Card> affectedCards = new List<Card>(e.SelectedCards);
                    affectedCards.Remove(card);
                    card.ApplyModifier(affectedCards.Count - 1, _playerCombatManager, _selectedEnemyCombatManagers, affectedCards);
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
            StartEnemyTurn(_enemyBehaviorManagers[e.EnemyIndex]);
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

        private void OnEnemyAbilityUsed(object sender, EventArgs e)
        {
            if(CheckDefeatConditions())
            {
                ((EnemyBehaviorManager)sender).Stop();
                EndBattle(false);
            }
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

        private void StartEnemyTurn(EnemyBehaviorManager enemyBehaviorManager)
        {
            _playerCombatManager.UpdateBonusesEffects();
            enemyBehaviorManager.EnemyCombatManager.ApplyPeriodicalEffects();
            enemyBehaviorManager.MakeMove();
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
            PlayerCharacterParamsModel playerCharacterParamsModel = (PlayerCharacterParamsModel)_playerCombatManager.GetParams(); 
            return playerCharacterParamsModel.HealthPoints.CurrentValue <= 0 || (playerCharacterParamsModel.PatientHealthPoints.CurrentValue <= 0 && !_combatUIManager.PlayerHasAnyCardsOnHands());
        }

        private void EndBattle(bool victory)
        {
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
