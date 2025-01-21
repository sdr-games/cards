using System;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.AbilitiesQueueModule.Managers;
using SDRGames.Whist.AIBehaviorModule.Managers;
using SDRGames.Whist.CardsCombatModule.Managers;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.MeleeCombatModule.Managers;
using SDRGames.Whist.RestorationModule.Managers;
using SDRGames.Whist.TurnSwitchModule.Managers;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.DomainModule.Managers
{
    public class CombatSceneManager : MonoBehaviour
    {
        [SerializeField] private UserInputController _userInputController;
        [SerializeField] private TurnsQueueManager _turnsQueueManager;

        //[Header("MELEE ABILITIES")][SerializeField] private AbilitiesQueueManager _abilitiesQueueManager;
        //[SerializeField] private MeleeAttackListManager _meleeAttackListManager;

        //[Header("CARDS")][SerializeField] private SelectedDeckManager _selectedDeckManager;
        //[SerializeField] private DecksPreviewWindowManager _decksPreviewWindowManager;
        //[SerializeField] private DeckOnHandsManager _deckOnHandsManager;

        //[Header("RESTORATION")][SerializeField] private PotionListManager _potionListManager;
        [Header("UI")][SerializeField] private CombatUIManager _combatUIManager;

        [Header("PLAYER")][SerializeField] private PlayerCombatManager _playerCombatManager;
        //[SerializeField] private HideableUIView _playerSwitchableUI;

        [Header("ENEMIES")][SerializeField] private EnemyBehaviorManager[] _enemyBehaviorManagers;
        //[SerializeField] private CanvasGroup _enemySwitchableUI;

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_userInputController), _userInputController);
            this.CheckFieldValueIsNotNull(nameof(_turnsQueueManager), _turnsQueueManager);
            this.CheckFieldValueIsNotNull(nameof(_playerCombatManager), _playerCombatManager);
            this.CheckFieldValueIsNotNull(nameof(_enemyBehaviorManagers), _enemyBehaviorManagers);

            //if (_abilitiesQueueManager == null)
            //{
            //    Debug.LogError("Abilities Queue Manager не был назначен");
            //    #if UNITY_EDITOR
            //        EditorApplication.isPlaying = false;
            //    #endif
            //    Application.Quit();
            //}

            //if (_meleeAttackListManager == null)
            //{
            //    Debug.LogError("Attack List Manager не был назначен");
            //    #if UNITY_EDITOR
            //        EditorApplication.isPlaying = false;
            //    #endif
            //    Application.Quit();
            //}

            //if (_selectedDeckManager == null)
            //{
            //    Debug.LogError("Selected Deck Manager не был назначен");
            //    #if UNITY_EDITOR
            //        EditorApplication.isPlaying = false;
            //    #endif
            //    Application.Quit();
            //}

            //if (_decksPreviewWindowManager == null)
            //{
            //    Debug.LogError("Decks Preview Window Manager не был назначен");
            //    #if UNITY_EDITOR
            //        EditorApplication.isPlaying = false;
            //    #endif
            //    Application.Quit();
            //}

            //if (_deckOnHandsManager == null)
            //{
            //    Debug.LogError("Deck On Hands Manager не был назначен");
            //    #if UNITY_EDITOR
            //        EditorApplication.isPlaying = false;
            //    #endif
            //    Application.Quit();
            //}

            //if (_potionListManager == null)
            //{
            //    Debug.LogError("Potion List Manager не был назначен");
            //    #if UNITY_EDITOR
            //        EditorApplication.isPlaying = false;
            //    #endif
            //    Application.Quit();
            //}

            //if (_playerSwitchableUI == null)
            //{
            //    Debug.LogError("Player Switchable UI не был назначен");
            //    #if UNITY_EDITOR
            //        EditorApplication.isPlaying = false;
            //    #endif
            //    Application.Quit();
            //}

            //if (_enemyMeleeBehaviorManager == null)
            //{
            //    Debug.LogError("Enemy Melee Behavior Manager не был назначен");
            //    #if UNITY_EDITOR
            //        EditorApplication.isPlaying = false;
            //    #endif
            //    Application.Quit();
            //}

            _playerCombatManager.Initialize();

            List<CharacterParamsModel> characterInfoScriptableObjects = new List<CharacterParamsModel>();
            List<EnemyCombatManager> enemyCombatManagers = new List<EnemyCombatManager>();
            characterInfoScriptableObjects.Add(_playerCombatManager.GetParams());
            foreach (EnemyBehaviorManager enemyBehaviorManager in _enemyBehaviorManagers)
            {
                enemyBehaviorManager.Initialize(_playerCombatManager);
                enemyCombatManagers.Add(enemyBehaviorManager.EnemyCombatManager);
                characterInfoScriptableObjects.Add(enemyBehaviorManager.EnemyCombatManager.GetParams());   
            }

            //_enemyMeleeBehaviorManager.Initialize(_enemyCharacterCombatManager, _playerCharacterCombatManager);

            _turnsQueueManager.TurnSwitched += OnTurnSwitched;
            _turnsQueueManager.Initialize(characterInfoScriptableObjects);

            _combatUIManager.Initialize(
                _userInputController, 
                _turnsQueueManager, 
                _playerCombatManager,
                enemyCombatManagers
            );
            _combatUIManager.AbilityQueueCleared += OnAbilityQueueCleared;
            _combatUIManager.CardClicked += OnCardClicked;
            _combatUIManager.MeleeAttackClicked += OnMeleeAttackClicked;
            _combatUIManager.CardsTurnEnd += OnCardsTurnEnd;
            _combatUIManager.MeleeTurnEnd += OnMeleeTurnEnd;

            //_abilitiesQueueManager.Initialize(_userInputController);
            //_abilitiesQueueManager.ApplyButtonClicked += OnQueueApplyButtonClicked;
            //_abilitiesQueueManager.AbilityQueueCleared += OnAbilityQueueCleared;

            //_meleeAttackListManager.Initialize(_userInputController);
            //_meleeAttackListManager.MeleeAttackClicked += OnMeleeAttackClicked;

            //_potionListManager.Initialize(_userInputController);
            //_potionListManager.PotionClicked += OnPotionClicked;

            //_selectedDeckManager.Initialize(_userInputController);
            //_selectedDeckManager.EmptyDeckViewClicked += OnEmptyDeckViewClicked;
            //_selectedDeckManager.SelectedDeckViewClicked += OnSelectedDeckViewClicked;

            //_decksPreviewWindowManager.Initialize(_userInputController);
            //_decksPreviewWindowManager.DeckSelected += OnDeckSelected;

            //_deckOnHandsManager.Initialize(_userInputController);
            //_deckOnHandsManager.CardClicked += OnCardClicked;

        }

        private void OnCardClicked(object sender, CardClickedEventArgs e)
        {
            if(!e.IsSelected && _playerCombatManager.HasEnoughBreathPoints(e.CardManager.CardScriptableObject.Cost))
            {
                _combatUIManager.SelectCard(e.CardManager);
                return;
            }
            _combatUIManager.DeselectCard(e.CardManager);
        }

        private void OnMeleeAttackClicked(object sender, MeleeAttackClickedEventArgs e)
        {
            if(!_playerCombatManager.HasEnoughStaminaPoints(e.MeleeAttackScriptableObject.Cost))
            {
                return;
            }

            if (_combatUIManager.TryAddAbilityToQueue(e.MeleeAttackScriptableObject))
            {
                _playerCombatManager.ReserveStaminaPoints(e.MeleeAttackScriptableObject.Cost);
            }            
        }

        private void OnMeleeTurnEnd(object sender, MeleeEndTurnEventArgs e)
        {
            
            List<CharacterCombatManager> enemyCharacterCombatManagers = new List<CharacterCombatManager>(GetEnemyCombatManagers());

            foreach (var ability in e.Abilities)
            {
                if (ability == null)
                {
                    continue;
                }
                ability.ApplyLogics(
                    _playerCombatManager,
                    enemyCharacterCombatManagers,
                    e.Abilities.Count,
                    new List<int>() { 0 });
            }
            _playerCombatManager.SpendStaminaPoints(e.TotalCost);
            _turnsQueueManager.SwitchTurn();
        }

        //private void OnDeckApplyButtonClicked(object sender, CardsCombatModule.Managers.ApplyButtonClickedEventArgs e)
        //{
        //    if (e.CardManagers.Count == 0)
        //    {
        //        return;
        //    }

        //    List<CharacterCombatManager> enemyCharacterCombatManagers = new List<CharacterCombatManager>() { _enemyCharacterCombatManagers };

        //    foreach (CardManager cardManager in e.CardManagers)
        //    {
        //        _deckOnHandsManager.RemoveSelectedCard(cardManager);

        //    }
        //}

        private void OnCardsTurnEnd(object sender, CardsEndTurnEventArgs e)
        {
            //if (e.Managers.Count == 0)
            //{
            //    return;
            //}

            //List<CharacterCombatManager> enemyCharacterCombatManagers = new List<CharacterCombatManager>() { _enemyCharacterCombatManager };

            //List<CardScriptableObject> cards = new List<CardScriptableObject>();
            //foreach (CardManager card in e.Managers)
            //{
            //    cards.Add(card.CardScriptableObject);
            //}

            //foreach (CardManager card in e.Managers)
            //{
            //    if (card == null)
            //    {
            //        continue;
            //    }
            //    _deckOnHandsManager.RemoveSelectedCard(card);

            //    if (card.CardScriptableObject.AbilityModifiers.Length > 0)
            //    {
            //        card.CardScriptableObject.ApplyModifiers(_playerCharacterCombatManager,
            //        enemyCharacterCombatManagers,
            //        e.Managers.Count,
            //        cards
            //        );
            //    }

            //    card.CardScriptableObject.ApplyLogics(
            //        _playerCharacterCombatManager,
            //        enemyCharacterCombatManagers,
            //        e.Managers.Count,
            //        new List<int>() { 0 });
            //}
            //_playerCharacterCombatManager.SpendBreathPoints(e.TotalCost);
            //_turnsQueueManager.SwitchTurn();
        }

        private void OnAbilityQueueCleared(object sender, AbilityQueueClearedEventArgs e)
        {
            _playerCombatManager.ResetStaminaReservedPoints(e.ReverseAmount);
        }

        private void OnTurnSwitched(object sender, TurnSwitchedEventArgs e)
        {
            _combatUIManager.HidePlayerUI();
            if (e.IsPlayerTurn)
            {
                StartPlayerTurn(e.IsCombatTurn);
                return;
            }
            StartEnemyTurn(null);
        }

        private void StartPlayerTurn(bool isCombatTurn)
        {
            _playerCombatManager.ApplyPeriodicalEffects();
            foreach (EnemyCombatManager enemyCharacterCombatManager in GetEnemyCombatManagers())
            {
                enemyCharacterCombatManager.UpdateBonusesEffects();
            }
            _combatUIManager.ShowPlayerUI(isCombatTurn);
        }

        private void StartEnemyTurn(EnemyCombatManager enemyCharacterCombatManager)
        {
            enemyCharacterCombatManager.ApplyPeriodicalEffects();
            _playerCombatManager.UpdateBonusesEffects();
            //_enemyMeleeBehaviorManager.ChooseAndAppyAbilities();
            _turnsQueueManager.SwitchTurn();
        }

        private List<EnemyCombatManager> GetEnemyCombatManagers()
        {
            List<EnemyCombatManager> enemyCombatManagers = new List<EnemyCombatManager>();
            foreach(EnemyBehaviorManager enemyBehaviorManager in _enemyBehaviorManagers)
            {
                enemyCombatManagers.Add(enemyBehaviorManager.EnemyCombatManager);
            }
            return enemyCombatManagers;
        }
    }
}
