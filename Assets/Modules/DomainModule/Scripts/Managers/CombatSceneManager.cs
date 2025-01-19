using System;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.AbilitiesQueueModule.Managers;
using SDRGames.Whist.CardsCombatModule.Managers;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.MeleeCombatModule.AI.Managers;
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

        [Header("PLAYER")][SerializeField] private PlayerCharacterCombatManager _playerCharacterCombatManager;
        //[SerializeField] private HideableUIView _playerSwitchableUI;

        [Header("ENEMY")][SerializeField] private EnemyCharacterCombatManager _enemyCharacterCombatManager;
        //[SerializeField] private EnemyMeleeBehaviorManager _enemyMeleeBehaviorManager;
        //[SerializeField] private CanvasGroup _enemySwitchableUI;

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_userInputController), _userInputController);
            this.CheckFieldValueIsNotNull(nameof(_turnsQueueManager), _turnsQueueManager);
            this.CheckFieldValueIsNotNull(nameof(_playerCharacterCombatManager), _playerCharacterCombatManager);
            this.CheckFieldValueIsNotNull(nameof(_enemyCharacterCombatManager), _enemyCharacterCombatManager);

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

            _playerCharacterCombatManager.Initialize();
            _enemyCharacterCombatManager.Initialize();
            _enemyCharacterCombatManager.InitializeBehavior();

            //_enemyMeleeBehaviorManager.Initialize(_enemyCharacterCombatManager, _playerCharacterCombatManager);

            List<CharacterParamsModel> characterInfoScriptableObjects = new List<CharacterParamsModel>();
            characterInfoScriptableObjects.Add(_playerCharacterCombatManager.GetParams());
            characterInfoScriptableObjects.Add(_enemyCharacterCombatManager.GetParams());
            _turnsQueueManager.TurnSwitched += OnTurnSwitched;
            _turnsQueueManager.Initialize(characterInfoScriptableObjects);

            _combatUIManager.Initialize(
                _userInputController, 
                _turnsQueueManager, 
                _playerCharacterCombatManager, 
                _enemyCharacterCombatManager
            );
            _combatUIManager.CardClicked += OnCardClicked;

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
            if(!e.IsSelected && _playerCharacterCombatManager.HasEnoughBreathPoints(e.CardManager.CardScriptableObject.Cost))
            {
                _combatUIManager.TrySelectCard(e.CardManager);
                return;
            }
            _combatUIManager.TryDeselectCard(e.CardManager);
        }

        private void OnMeleeAttackClicked(object sender, MeleeAttackClickedEventArgs e)
        {
            if(_abilitiesQueueManager.IsFull || !_playerCharacterCombatManager.HasEnoughStaminaPoints(e.MeleeAttackScriptableObject.Cost))
            {
                return;
            }
            _playerCharacterCombatManager.ReserveStaminaPoints(e.MeleeAttackScriptableObject.Cost);
            _abilitiesQueueManager.AddAbilityToQueue(e.MeleeAttackScriptableObject);
        }

        private void OnPotionClicked(object sender, PotionClickedEventArgs e)
        {
            if (_abilitiesQueueManager.IsFull)
            {
                return;
            }
            _abilitiesQueueManager.AddAbilityToQueue(e.PotionScriptableObject);
        }

        private void OnQueueApplyButtonClicked(object sender, AbilitiesQueueModule.Managers.ApplyButtonClickedEventArgs e)
        {
            if(e.AbilityScriptableObjects.Count == 0 && e.AbilityScriptableObjects.Any(item => item is null))
            {
                return;
            }

            List<CharacterCombatManager> enemyCharacterCombatManagers = new List<CharacterCombatManager>() { _enemyCharacterCombatManager };

            foreach (var ability in e.AbilityScriptableObjects)
            {
                if(ability == null)
                {
                    continue;
                }
                ability.ApplyLogics(
                    _playerCharacterCombatManager,
                    enemyCharacterCombatManagers, 
                    e.AbilityScriptableObjects.Count,
                    new List<int>() { 0 });
            }
            _playerCharacterCombatManager.SpendStaminaPoints(e.TotalCost);
            _turnsQueueManager.SwitchTurn();
        }

        private void OnDeckApplyButtonClicked(object sender, CardsCombatModule.Managers.ApplyButtonClickedEventArgs e)
        {
            if(e.CardManagers.Count == 0)
            {
                return;
            }

            List<CharacterCombatManager> enemyCharacterCombatManagers = new List<CharacterCombatManager>() { _enemyCharacterCombatManager };

            foreach(CardManager cardManager in e.CardManagers)
            {
                _deckOnHandsManager.RemoveSelectedCard(cardManager);

            }
        }

        //private void OnDeckApplyButtonClicked(object sender, CardsCombatModule.Managers.ApplyButtonClickedEventArgs e)
        //{
        //    if(e.Managers.Count == 0)
        //    {
        //        return;
        //    }

        //    List<CharacterCombatManager> enemyCharacterCombatManagers = new List<CharacterCombatManager>() { _enemyCharacterCombatManager };

        //    List<CardScriptableObject> cards = new List<CardScriptableObject>();
        //    foreach(CardManager card in e.Managers)
        //    {
        //        cards.Add(card.CardScriptableObject);
        //    }

        //    foreach (CardManager card in e.Managers)
        //    {
        //        if (card == null)
        //        {
        //            continue;
        //        }
        //        _deckOnHandsManager.RemoveSelectedCard(card);
                
        //        if(card.CardScriptableObject.AbilityModifiers.Length > 0)
        //        {
        //            card.CardScriptableObject.ApplyModifiers(_playerCharacterCombatManager,
        //            enemyCharacterCombatManagers,
        //            e.Managers.Count,
        //            cards
        //            );
        //        }

        //        card.CardScriptableObject.ApplyLogics(
        //            _playerCharacterCombatManager,
        //            enemyCharacterCombatManagers,
        //            e.Managers.Count,
        //            new List<int>() { 0 });
        //    }
        //    _playerCharacterCombatManager.SpendBreathPoints(e.TotalCost);
        //    _turnsQueueManager.SwitchTurn();
        //}

        private void OnAbilityQueueCleared(object sender, AbilityQueueClearedEventArgs e)
        {
            _playerCharacterCombatManager.ResetStaminaReservedPoints(e.ReverseAmount);
        }

        private void OnSelectedDeckViewClicked(object sender, EventArgs e)
        {
            _abilitiesQueueManager.Hide();
            _meleeAttackListManager.Hide();
            _potionListManager.Hide();
            _deckOnHandsManager.ShowView();
        }

        private void OnEmptyDeckViewClicked(object sender, EventArgs e)
        {
            _abilitiesQueueManager.Hide();
            _meleeAttackListManager.Hide();
            _decksPreviewWindowManager.Show();
        }

        private void OnDeckSelected(object sender, DeckPreviewClickedEventArgs e)
        {
            _abilitiesQueueManager.Show();
            _meleeAttackListManager.Show();
            _decksPreviewWindowManager.Hide();
            _selectedDeckManager.SetSelectedDeck(e.DeckScriptableObject);
            _deckOnHandsManager.SetSelectedDeck(e.DeckScriptableObject);
        }

        private void OnTurnSwitched(object sender, TurnSwitchedEventArgs e)
        {
            _combatUIManager.HidePlayerUI();
            if (e.IsPlayerTurn)
            {
                StartPlayerTurn(e.IsCombatTurn);
                return;
            }
            StartEnemyTurn();
        }

        private void StartPlayerTurn(bool isCombatTurn)
        {
            _playerCharacterCombatManager.ApplyPeriodicalEffects();
            _enemyCharacterCombatManager.UpdateBonusesEffects();
            _combatUIManager.ShowPlayerUI(isCombatTurn);
        }

        private void StartEnemyTurn()
        {
            _enemyCharacterCombatManager.ApplyPeriodicalEffects();
            _playerCharacterCombatManager.UpdateBonusesEffects();
            _enemyMeleeBehaviorManager.ChooseAndAppyAbilities();
            _turnsQueueManager.SwitchTurn();
        }
    }
}
