using System;
using System.Collections.Generic;

using SDRGames.Whist.AbilitiesQueueModule.Managers;
using SDRGames.Whist.CardsCombatModule.Managers;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.MeleeCombatModule.Managers;
using SDRGames.Whist.RestorationModule.Managers;
using SDRGames.Whist.TurnSwitchModule.Managers;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.DomainModule.Managers
{
    public class CombatUIInitializer : MonoBehaviour
    {
        [SerializeField] private UserInputController _userInputController;
        [SerializeField] private TurnsQueueManager _turnsQueueManager;

        [Header("MELEE ABILITIES")][SerializeField] private AbilitiesQueueManager _abilitiesQueueManager;
        [SerializeField] private MeleeAttackListManager _meleeAttackListManager;

        [Header("CARDS")][SerializeField] private SelectedDeckManager _selectedDeckManager;
        [SerializeField] private DecksPreviewWindowManager _decksPreviewWindowManager;
        [SerializeField] private DeckOnHandsManager _deckOnHandsManager;

        [Header("RESTORATION")][SerializeField] private PotionListManager _potionListManager;

        [Header("PLAYER")][SerializeField] private PlayerCharacterCombatManager _playerCharacterCombatManager;
        [SerializeField] private HideableUIView _playerSwitchableUI;

        [Header("ENEMY")][SerializeField] private EnemyCharacterCombatManager _enemyCharacterCombatManager;
        //[SerializeField] private CanvasGroup _enemySwitchableUI;

        private void OnEnable()
        {
            if (_userInputController == null)
            {
                Debug.LogError("User Input Controller не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_turnsQueueManager == null)
            {
                Debug.LogError("Turns Queue Manager не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_abilitiesQueueManager == null)
            {
                Debug.LogError("Abilities Queue Manager не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_meleeAttackListManager == null)
            {
                Debug.LogError("Attack List Manager не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_selectedDeckManager == null)
            {
                Debug.LogError("Selected Deck Manager не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_decksPreviewWindowManager == null)
            {
                Debug.LogError("Decks Preview Window Manager не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_deckOnHandsManager == null)
            {
                Debug.LogError("Deck On Hands Manager не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_potionListManager == null)
            {
                Debug.LogError("Potion List Manager не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_playerCharacterCombatManager == null)
            {
                Debug.LogError("Player Character Combat Manager не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_playerSwitchableUI == null)
            {
                Debug.LogError("Player Switchable UI не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_enemyCharacterCombatManager == null)
            {
                Debug.LogError("Enemy Character Combat Manager не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            _playerCharacterCombatManager.Initialize();
            _enemyCharacterCombatManager.Initialize();

            _abilitiesQueueManager.Initialize(_userInputController);
            _abilitiesQueueManager.ApplyButtonClicked += OnQueueApplyButtonClicked;
            _abilitiesQueueManager.AbilityQueueCleared += OnAbilityQueueCleared;

            _meleeAttackListManager.Initialize(_userInputController);
            _meleeAttackListManager.MeleeAttackClicked += OnMeleeAttackClicked;

            _potionListManager.Initialize(_userInputController);
            _potionListManager.PotionClicked += OnPotionClicked;

            _selectedDeckManager.Initialize(_userInputController);
            _selectedDeckManager.EmptyDeckViewClicked += OnEmptyDeckViewClicked;
            _selectedDeckManager.SelectedDeckViewClicked += OnSelectedDeckViewClicked;

            _decksPreviewWindowManager.Initialize(_userInputController);
            _decksPreviewWindowManager.DeckSelected += OnDeckSelected;

            _deckOnHandsManager.Initialize(_userInputController);
            _deckOnHandsManager.CardClicked += OnCardClicked;
            _deckOnHandsManager.ApplyButtonClicked += OnDeckApplyButtonClicked;

            List<CharacterParamsModel> characterInfoScriptableObjects = new List<CharacterParamsModel>();
            characterInfoScriptableObjects.Add(_playerCharacterCombatManager.GetParams());
            characterInfoScriptableObjects.Add(_enemyCharacterCombatManager.GetParams());
            _turnsQueueManager.TurnSwitched += OnTurnSwitched;
            _turnsQueueManager.Initialize(characterInfoScriptableObjects);

            _playerCharacterCombatManager.TakePhysicalDamage(16);
        }

        private void ShowPlayerUI(bool isCombatTurn)
        {
            if (isCombatTurn)
            {
                _meleeAttackListManager.Show();
                _potionListManager.Hide();
            }
            else
            {
                _meleeAttackListManager.Hide();
                _potionListManager.Show();
            }
            _playerSwitchableUI.Show();
        }

        private void HidePlayerUI()
        {
            _playerSwitchableUI.Hide();
            _deckOnHandsManager.Hide();
            _potionListManager.Hide();
            _decksPreviewWindowManager.Hide();
        }

        private void OnCardClicked(object sender, CardClickedEventArgs e)
        {
            if(!e.IsSelected && _playerCharacterCombatManager.HasEnoughBreathPoints(e.CardManager.CardScriptableObject.Cost))
            {
                _deckOnHandsManager.AddSelectedCards(e.CardManager);
                _playerCharacterCombatManager.ReserveBreathPoints(e.CardManager.CardScriptableObject.Cost);
                return;
            }
            if (_deckOnHandsManager.RemoveSelectedCard(e.CardManager))
            {
                _playerCharacterCombatManager.ResetBreathReservedPoints(e.CardManager.CardScriptableObject.Cost);
            }
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
            if(e.AbilityScriptableObjects.Count == 0)
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
                    new List<int>() { 0 });
            }
            _playerCharacterCombatManager.SpendStaminaPoints(e.TotalCost);
            HidePlayerUI();
            _turnsQueueManager.SwitchTurn();
        }

        private void OnDeckApplyButtonClicked(object sender, CardsCombatModule.Managers.ApplyButtonClickedEventArgs e)
        {
            if(e.Cards.Count == 0)
            {
                return;
            }

            foreach (var card in e.Cards)
            {
                if (card == null)
                {
                    continue;
                }
                _deckOnHandsManager.RemoveSelectedCard(card);
                //TODO: Apply card behavior
            }
            _playerCharacterCombatManager.SpendBreathPoints(e.TotalCost);
            HidePlayerUI();
            _turnsQueueManager.SwitchTurn();
        }

        private void OnAbilityQueueCleared(object sender, AbilityQueueClearedEventArgs e)
        {
            _playerCharacterCombatManager.ResetStaminaReservedPoints(e.ReverseAmount);
        }

        private void OnSelectedDeckViewClicked(object sender, EventArgs e)
        {
            _abilitiesQueueManager.Hide();
            _meleeAttackListManager.Hide();
            _potionListManager.Hide();
            _deckOnHandsManager.Show();
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
            HidePlayerUI();
            if (e.IsPlayerTurn)
            {
                ShowPlayerUI(e.IsCombatTurn);
                return;
            }
        }
    }
}
