using System.Collections.Generic;
using System.Linq;
using System;

using SDRGames.Whist.AbilitiesQueueModule.Managers;
using SDRGames.Whist.CardsCombatModule.Managers;
using SDRGames.Whist.DomainModule.Views;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.MeleeCombatModule.Managers;
using SDRGames.Whist.RestorationModule.Managers;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;
using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.AbilitiesModule.Models;

namespace SDRGames.Whist.DomainModule.Managers
{
    public class CombatUIManager : MonoBehaviour
    {
        private UserInputController _userInputController;

        [SerializeField] private CombatUIView _combatUIView;

        [Header("MELEE ABILITIES")][SerializeField] private AbilitiesQueueManager _abilitiesQueueManager;
        [SerializeField] private MeleeAttackListManager _meleeAttackListManager;

        [Header("CARDS")][SerializeField] private SelectedDeckManager _selectedDeckManager;
        [SerializeField] private DecksPreviewWindowManager _decksPreviewWindowManager;
        [SerializeField] private DeckOnHandsManager _deckOnHandsManager;

        [Header("RESTORATION")][SerializeField] private PotionListManager _potionListManager;

        [Header("PLAYER")][SerializeField] private HideableUIView _playerSwitchableUI;

        public event EventHandler<CardClickedEventArgs> CardClicked;
        public event EventHandler<MeleeAttackClickedEventArgs> MeleeAttackClicked;
        public event EventHandler<AbilityQueueClearedEventArgs> AbilityQueueCleared;
        public event EventHandler<CardsEndTurnEventArgs> CardsTurnEnd;
        public event EventHandler<MeleeEndTurnEventArgs> MeleeTurnEnd;
        public event EventHandler<CardsSelectionClearedEventArgs> CardsSelectionCleared;
        public event EventHandler ClearButtonClicked;

        public void Initialize(UserInputController userInputController)
        {
            _userInputController = userInputController;

            _abilitiesQueueManager.Initialize(_userInputController);
            _abilitiesQueueManager.AbilityQueueCleared += OnAbilityQueueCleared;
            _abilitiesQueueManager.AbilityQueueCountChanged += _combatUIView.OnAbilityQueueCountChanged;

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
            _deckOnHandsManager.CardsSelectionCleared += OnCardsSelectionCleared;
            _deckOnHandsManager.SelectedCardsCountChanged += _combatUIView.OnSelectedCardsCountChanged;

            _combatUIView.Initialize(_userInputController);
            _combatUIView.EndTurnButtonClicked += OnEndTurnButtonClicked;
            _combatUIView.ClearButtonClicked += OnClearButtonClicked;

            HidePlayerUI();
        }

        public void ShowPlayerUI(bool isCombatTurn)
        {
            _abilitiesQueueManager.Show();
            if (isCombatTurn)
            {
                _meleeAttackListManager.Show();
                _selectedDeckManager.Show();
                _potionListManager.Hide();
            }
            else
            {
                _meleeAttackListManager.Hide();
                _deckOnHandsManager.HideView();
                _selectedDeckManager.Hide();
                _potionListManager.Show();
            }
            _playerSwitchableUI.Show();
        }

        public void HidePlayerUI()
        {
            _abilitiesQueueManager.ClearBindedAbilities();
            _deckOnHandsManager.ClearCardsSelection();
            _abilitiesQueueManager.Hide();
            _playerSwitchableUI.Hide();
            _deckOnHandsManager.HideView();
            _potionListManager.Hide();
            _decksPreviewWindowManager.Hide();
        }

        public bool TrySelectCard(CardManager cardManager)
        {
            return _deckOnHandsManager.TryAddSelectedCard(cardManager);       
        }

        public bool TryDeselectCard(CardManager cardManager)
        {
            return _deckOnHandsManager.TryDeselectCard(cardManager);
        }

        public bool TryRemoveCard(CardManager cardManager)
        {
            return _deckOnHandsManager.TryRemoveSelectedCard(cardManager);
        } 

        public bool TryAddAbilityToQueue(Ability ability)
        {
            return _abilitiesQueueManager.TryAddAbilityToQueue(ability);
        }

        #region Events methods

        private void OnCardClicked(object sender, CardClickedEventArgs e)
        {
            CardClicked?.Invoke(this, e);
        }

        private void OnCardsSelectionCleared(object sender, CardsSelectionClearedEventArgs e)
        {
            CardsSelectionCleared?.Invoke(this, e);
        }

        private void OnMeleeAttackClicked(object sender, MeleeAttackClickedEventArgs e)
        {
            MeleeAttackClicked?.Invoke(this, e);
        }

        private void OnPotionClicked(object sender, PotionClickedEventArgs e)
        {
            if (_abilitiesQueueManager.IsFull)
            {
                return;
            }
            TryAddAbilityToQueue(e.Potion);
        }

        private void OnAbilityQueueCleared(object sender, AbilityQueueClearedEventArgs e)
        {
            AbilityQueueCleared?.Invoke(this, e);
        }

        private void OnSelectedDeckViewClicked(object sender, SelectedDeckViewClickedEventArgs e)
        {
            if (!e.Visible)
            {
                _deckOnHandsManager.ClearCardsSelection();
                _abilitiesQueueManager.Show();
                _meleeAttackListManager.Show();
                _deckOnHandsManager.HideView();
                return;
            }
            _abilitiesQueueManager.ClearBindedAbilities();
            _abilitiesQueueManager.Hide();
            _meleeAttackListManager.Hide();
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

        private void OnEndTurnButtonClicked(object sender, EventArgs e)
        {
            float totalCost = 0;

            List<Card> selectedCards = _deckOnHandsManager.PopSelectedCards();
            if (selectedCards != null)
            {
                HidePlayerUI();
                totalCost = selectedCards.Where(item => item != null).Sum(item => item.Cost);
                CardsTurnEnd?.Invoke(this, new CardsEndTurnEventArgs(totalCost, selectedCards));
                return;
            }

            List<Ability> selectedAbilities = _abilitiesQueueManager.PopSelectedAbilities();
            HidePlayerUI();
            MeleeTurnEnd?.Invoke(this, new MeleeEndTurnEventArgs(selectedAbilities));
        }

        private void OnClearButtonClicked(object sender, EventArgs e)
        {
            _abilitiesQueueManager.ClearBindedAbilities();
            ClearButtonClicked?.Invoke(this, e);
        }

        #endregion

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_combatUIView), _combatUIView);
            this.CheckFieldValueIsNotNull(nameof(_abilitiesQueueManager), _abilitiesQueueManager);
            this.CheckFieldValueIsNotNull(nameof(_meleeAttackListManager), _meleeAttackListManager);
            this.CheckFieldValueIsNotNull(nameof(_selectedDeckManager), _selectedDeckManager);
            this.CheckFieldValueIsNotNull(nameof(_decksPreviewWindowManager), _decksPreviewWindowManager);
            this.CheckFieldValueIsNotNull(nameof(_deckOnHandsManager), _deckOnHandsManager);
            this.CheckFieldValueIsNotNull(nameof(_potionListManager), _potionListManager);
            this.CheckFieldValueIsNotNull(nameof(_playerSwitchableUI), _playerSwitchableUI);
        }

        private void OnDestroy()
        {
            _abilitiesQueueManager.AbilityQueueCleared -= OnAbilityQueueCleared;
            _abilitiesQueueManager.AbilityQueueCountChanged -= _combatUIView.OnAbilityQueueCountChanged;
            _meleeAttackListManager.MeleeAttackClicked -= OnMeleeAttackClicked;
            _potionListManager.PotionClicked -= OnPotionClicked;
            _selectedDeckManager.EmptyDeckViewClicked -= OnEmptyDeckViewClicked;
            _selectedDeckManager.SelectedDeckViewClicked -= OnSelectedDeckViewClicked;
            _decksPreviewWindowManager.DeckSelected -= OnDeckSelected;
            _deckOnHandsManager.CardClicked -= OnCardClicked;
            _deckOnHandsManager.SelectedCardsCountChanged -= _combatUIView.OnSelectedCardsCountChanged;
            _combatUIView.EndTurnButtonClicked -= OnEndTurnButtonClicked;
            _combatUIView.ClearButtonClicked -= OnClearButtonClicked;
        }
    }
}
