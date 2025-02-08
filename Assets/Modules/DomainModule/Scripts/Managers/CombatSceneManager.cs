using System;
using System.Collections.Generic;

using SDRGames.Whist.AbilitiesQueueModule.Managers;
using SDRGames.Whist.AIBehaviorModule.Managers;
using SDRGames.Whist.CardsCombatModule.Managers;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.MeleeCombatModule.Managers;
using SDRGames.Whist.TurnSwitchModule.Managers;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;
using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;
using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.CardsCombatModule.Models;

namespace SDRGames.Whist.DomainModule.Managers
{
    public class CombatSceneManager : MonoBehaviour
    {
        [SerializeField] private UserInputController _userInputController;
        [SerializeField] private TurnsQueueManager _turnsQueueManager;
        [SerializeField] private CharacterParametersScalingScriptableObject _characterParametersScalingSettings;
        [SerializeField] private CardsScalingScriptableObject _cardsScalingScriptableObject;
        [SerializeField] private MeleeAttacksScalingScriptableObject _meleeAttacksScalingScriptableObject;

        [Header("UI")][SerializeField] private CombatUIManager _combatUIManager;
        [Header("PLAYER")][SerializeField] private PlayerCombatManager _playerCombatManager;
        [Header("ENEMIES")][SerializeField] private EnemyBehaviorManager[] _enemyBehaviorManagers;

        private List<EnemyCombatManager> _enemyCombatManagers;

        private void OnValidate()
        {
            _characterParametersScalingSettings.Initialize();
            _cardsScalingScriptableObject.Initialize();
            _meleeAttacksScalingScriptableObject.Initialize();
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_userInputController), _userInputController);
            this.CheckFieldValueIsNotNull(nameof(_turnsQueueManager), _turnsQueueManager);
            this.CheckFieldValueIsNotNull(nameof(_characterParametersScalingSettings), _characterParametersScalingSettings);
            this.CheckFieldValueIsNotNull(nameof(_cardsScalingScriptableObject), _cardsScalingScriptableObject);
            this.CheckFieldValueIsNotNull(nameof(_meleeAttacksScalingScriptableObject), _meleeAttacksScalingScriptableObject);
            this.CheckFieldValueIsNotNull(nameof(_playerCombatManager), _playerCombatManager);
            this.CheckFieldValueIsNotNull(nameof(_enemyBehaviorManagers), _enemyBehaviorManagers);

            _characterParametersScalingSettings.Initialize();
            _cardsScalingScriptableObject.Initialize();
            _meleeAttacksScalingScriptableObject.Initialize();

            _playerCombatManager.Initialize();
            _enemyCombatManagers = new List<EnemyCombatManager>();
            List<CharacterParamsModel> characterInfoScriptableObjects = new List<CharacterParamsModel>();
            characterInfoScriptableObjects.Add(_playerCombatManager.GetParams());
            foreach (EnemyBehaviorManager enemyBehaviorManager in _enemyBehaviorManagers)
            {
                enemyBehaviorManager.Initialize(_playerCombatManager);
                _enemyCombatManagers.Add(enemyBehaviorManager.EnemyCombatManager);
                characterInfoScriptableObjects.Add(enemyBehaviorManager.EnemyCombatManager.GetParams());   
            }

            _combatUIManager.Initialize(_userInputController);
            _combatUIManager.AbilityQueueCleared += OnAbilityQueueCleared;
            _combatUIManager.CardSelectClicked += OnCardSelectClicked;
            _combatUIManager.CardMarkClicked += OnCardMarkClicked;
            _combatUIManager.MeleeAttackClicked += OnMeleeAttackClicked;
            _combatUIManager.CardsTurnEnd += OnCardsTurnEnd;
            _combatUIManager.MeleeTurnEnd += OnMeleeTurnEnd;
            _combatUIManager.ClearButtonClicked += OnClearButtonClicked;
            _combatUIManager.CardsSelectionCleared += OnCardsSelectionCleared;

            _turnsQueueManager.Initialize(characterInfoScriptableObjects);
            _turnsQueueManager.TurnSwitched += OnTurnSwitched;
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
                    int cost = e.IsSelected ? -e.CardManager.Card.Cost * 2 : -e.CardManager.Card.Cost; // multiplication by 2 required because first we have to revert reserve by card's selection and after then add reserve by card's mark
                    _playerCombatManager.ReserveBreathPoints(cost);
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
            List<CharacterCombatManager> enemyCharacterCombatManagers = new List<CharacterCombatManager>(_enemyCombatManagers);

            foreach (Ability ability in e.Abilities)
            {
                if (ability == null)
                {
                    _playerCombatManager.RestoreStaminaPoints();
                    continue;
                }
                ability.ApplyLogics(
                    _playerCombatManager,
                    enemyCharacterCombatManagers,
                    e.Abilities.Count,
                    new List<int>() { 0 }
                );
                _playerCombatManager.SpendStaminaPoints(ability.Cost);
            }
            _turnsQueueManager.SwitchTurn();
        }

        private void OnCardsTurnEnd(object sender, CardsEndTurnEventArgs e)
        {
            List<CharacterCombatManager> enemyCharacterCombatManagers = new List<CharacterCombatManager>(_enemyCombatManagers);

            for(int i = 0; i < e.SelectedCards.Count; i++)
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
                    card.ApplyModifier(affectedCards.Count - 1, _playerCombatManager, enemyCharacterCombatManagers, affectedCards);
                    continue;
                }

                card.ApplyLogics(
                    _playerCombatManager,
                    enemyCharacterCombatManagers, 
                    e.SelectedCards.Count, 
                    new List<int>() { 0 }
                );
            }
            _playerCombatManager.SpendBreathPoints(e.TotalCost);
            _turnsQueueManager.SwitchTurn();
        }

        private void OnAbilityQueueCleared(object sender, AbilityQueueClearedEventArgs e)
        {
            _playerCombatManager.ResetStaminaReservedPoints(e.ReverseAmount);
        }

        private void OnTurnSwitched(object sender, TurnSwitchedEventArgs e)
        {
            if (e.IsPlayerTurn)
            {
                StartPlayerTurn(e.IsCombatTurn);
                return;
            }
            StartEnemyTurn(_enemyCombatManagers[0]);
        }

        private void OnClearButtonClicked(object sender, EventArgs e)
        {
            return;
        }

        private void OnCardsSelectionCleared(object sender, CardsSelectionClearedEventArgs e)
        {
            _playerCombatManager.ResetBreathReservedPoints(e.ReverseAmount);
        }

        private void StartPlayerTurn(bool isCombatTurn)
        {
            _playerCombatManager.ApplyPeriodicalEffects();
            foreach (EnemyCombatManager enemyCharacterCombatManager in _enemyCombatManagers)
            {
                enemyCharacterCombatManager.UpdateBonusesEffects();
            }
            _combatUIManager.ShowPlayerUI(isCombatTurn);
        }

        private void StartEnemyTurn(EnemyCombatManager enemyCharacterCombatManager)
        {
            enemyCharacterCombatManager?.ApplyPeriodicalEffects();
            _playerCombatManager.UpdateBonusesEffects();
            //_enemyMeleeBehaviorManager.ChooseAndAppyAbilities();
            _turnsQueueManager.SwitchTurn();
        }

        private void OnDestroy()
        {
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
