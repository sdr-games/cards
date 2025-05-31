using System;
using System.Linq;
using System.Collections.Generic;

using SDRGames.Whist.AbilitiesQueueModule.Managers;
using SDRGames.Whist.CardsCombatModule.Managers;
using SDRGames.Whist.DomainModule.Views;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.MeleeCombatModule.Managers;
using SDRGames.Whist.RestorationModule.Managers;
using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.CharacterInfoModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Models;
using SDRGames.Whist.CharacterCombatModule.Managers;

using UnityEngine;
using SDRGames.Whist.ActiveBlockModule.Managers;
using SDRGames.Whist.ActiveBlockModule.Views;

namespace SDRGames.Whist.DomainModule.Managers
{
    public class CombatUIManager : HideableUIView
    {
        private UserInputController _userInputController;

        [SerializeField] private CombatUIView _combatUIView;

        [Header("MELEE ABILITIES")][SerializeField] private AbilitiesQueueManager _abilitiesQueueManager;
        [SerializeField] private MeleeAttackListManager _meleeAttackListManager;
        [SerializeField] private ActiveBlockManager _activeBlockManager;

        [Header("CARDS")][SerializeField] private SelectedDeckManager _selectedDeckManager;
        [SerializeField] private DecksPreviewWindowManager _decksPreviewWindowManager;
        [SerializeField] private DeckOnHandsManager _deckOnHandsManager;

        [Header("RESTORATION")][SerializeField] private PotionListManager _potionListManager;

        [Header("PLAYER")][SerializeField] private HideableUIView _playerSwitchableUI; 

        public event EventHandler<CardSelectClickedEventArgs> CardSelectClicked;
        public event EventHandler<CardMarkClickedEventArgs> CardMarkClicked;
        public event EventHandler<MeleeAttackClickedEventArgs> MeleeAttackClicked;
        public event EventHandler<BlockKeyPressedCEventArgs> EnemyAttacksNotBlocked;
        public event EventHandler<AbilityQueueClearedEventArgs> AbilityQueueCleared;
        public event EventHandler<CardsEndTurnEventArgs> CardsTurnEnd;
        public event EventHandler<MeleeEndTurnEventArgs> MeleeTurnEnd;
        public event EventHandler<CardsSelectionClearedEventArgs> CardsSelectionCleared;
        public event EventHandler ClearButtonClicked;

        public void Initialize(UserInputController userInputController, PlayerScriptableObject playerScriptableObject, PlayerParamsModel playerParamsModel)
        {
            _userInputController = userInputController;

            _abilitiesQueueManager.Initialize(_userInputController);
            _abilitiesQueueManager.AbilityQueueCleared += OnAbilityQueueCleared;
            _abilitiesQueueManager.AbilityQueueCountChanged += _combatUIView.OnAbilityQueueCountChanged;

            _meleeAttackListManager.Initialize(_userInputController, playerScriptableObject.MeleeAttacks, playerParamsModel);
            _meleeAttackListManager.MeleeAttackClicked += OnMeleeAttackClicked;

            _activeBlockManager.Initialize();
            _activeBlockManager.BlockKeyPressed += OnBlockKeyPressed;
            
            _potionListManager.Initialize(_userInputController, playerParamsModel);
            _potionListManager.PotionClicked += OnPotionClicked;

            _selectedDeckManager.Initialize(_userInputController);
            _selectedDeckManager.EmptyDeckClicked += OnEmptyDeckClicked;
            _selectedDeckManager.SelectedDeckClicked += OnSelectedDeckClicked;

            _decksPreviewWindowManager.Initialize(_userInputController, playerScriptableObject.Decks);
            _decksPreviewWindowManager.DeckSelected += OnDeckSelected;

            _deckOnHandsManager.Initialize(_userInputController, playerParamsModel);
            _deckOnHandsManager.DeckUnsetted += OnDeckUnsetted;
            _deckOnHandsManager.CardSelectClicked += OnCardSelectClicked;
            _deckOnHandsManager.CardMarkClicked += OnCardMarkClicked;
            _deckOnHandsManager.CardsSelectionCleared += OnCardsSelectionCleared;
            _deckOnHandsManager.PickedCardsCountChanged += _combatUIView.OnSelectedCardsCountChanged;

            _combatUIView.Initialize(_userInputController);
            _combatUIView.EndTurnButtonClicked += OnEndTurnButtonClicked;
            _combatUIView.ClearButtonClicked += OnClearButtonClicked;

            HidePlayerUI();
        }

        private void OnBlockKeyPressed(object sender, BlockKeyPressedCEventArgs e)
        {
            _activeBlockManager.StopBlocking();
            EnemyAttacksNotBlocked?.Invoke(this, e);
        }

        public void ShowPlayerUI(bool isCombatTurn)
        {
            _abilitiesQueueManager.Show();
            if (isCombatTurn)
            {
                _meleeAttackListManager.Show();
                _selectedDeckManager.Show();
                _deckOnHandsManager.PullCardsFromDeck();
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
            _abilitiesQueueManager.ClearQueue();
            _deckOnHandsManager.ClearCardsQueue();
            _abilitiesQueueManager.Hide();
            _playerSwitchableUI.Hide();
            _deckOnHandsManager.HideView();
            _potionListManager.Hide();
            _selectedDeckManager.Hide();
            _decksPreviewWindowManager.Hide();
        }

        public void AddEnemyBars(EnemyCombatManager enemyCombatManager)
        {
            _combatUIView.SetAsParent(enemyCombatManager.GetView().transform);
            enemyCombatManager.GetView().Initialize(enemyCombatManager.transform);
        }

        public bool TrySelectCard(CardManager cardManager)
        {
            return _deckOnHandsManager.TryPickCard(cardManager);       
        }

        public bool TryDeselectCard(CardManager cardManager)
        {
            return _deckOnHandsManager.TryCancelPickCard(cardManager);
        }

        public bool TryMarkCard(CardManager cardManager)
        {
            return _deckOnHandsManager.TryMarkCardForDisenchant(cardManager);
        }

        public bool TryUnmarkCard(CardManager cardManager)
        {
            return _deckOnHandsManager.TryUnmarkCardForDisenchant(cardManager);
        }

        public bool TryRemoveCard(CardManager cardManager)
        {
            return _deckOnHandsManager.TryRemovePickedCard(cardManager);
        } 

        public bool TryAddAbilityToQueue(Ability ability)
        {
            return _abilitiesQueueManager.TryAddAbilityToQueue(ability);
        }

        public void RemoveUsedCards()
        {
            _deckOnHandsManager.RemoveUsedCards();
        }

        public void UnbindAbilitiesInQueue()
        {
            _abilitiesQueueManager.UnbindAbilities();
        }

        public void ShowNoTargetError()
        {
            _combatUIView.ShowNoTargetError();
        }

        public void ShowComaStartNotification()
        {
            _combatUIView.ShowComaStartNotification();
        }

        public void ShowComaStopNotification()
        {
            _combatUIView.ShowComaStopNotification();
        }

        public void ShowVictoryPanel()
        {
            _combatUIView.ShowVictoryPanel();
        }

        public void ShowDefeatPanel()
        {
            _combatUIView.ShowDefeatPanel();
        }

        public bool PlayerHasAnyCardsOnHands()
        {
            return _deckOnHandsManager.IsEmpty;
        }

        public void ShowActiveBlockingPanel(int chancesCount, float durationPerChance)
        {
            _activeBlockManager.StartBlocking(chancesCount, durationPerChance);
        }

        #region Events methods

        private void OnCardSelectClicked(object sender, CardSelectClickedEventArgs e)
        {
            CardSelectClicked?.Invoke(this, e);
        }

        private void OnCardMarkClicked(object sender, CardMarkClickedEventArgs e)
        {
            CardMarkClicked?.Invoke(this, e);
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

        private void OnSelectedDeckClicked(object sender, SelectedDeckViewClickedEventArgs e)
        {
            if (!e.Visible)
            {
                _deckOnHandsManager.ClearCardsQueue();
                _abilitiesQueueManager.Show();
                _meleeAttackListManager.Show();
                _deckOnHandsManager.HideView();
                return;
            }
            _abilitiesQueueManager.ClearQueue();
            _abilitiesQueueManager.Hide();
            _meleeAttackListManager.Hide();
            _deckOnHandsManager.ShowView();
        }

        private void OnEmptyDeckClicked(object sender, EventArgs e)
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
            _selectedDeckManager.SetSelectedDeck(e.DeckPreviewManager.Deck);
            _deckOnHandsManager.SetSelectedDeck(e.DeckPreviewManager.Deck);
        }

        private void OnDeckUnsetted(object sender, EventArgs e)
        {
            _selectedDeckManager.UnsetSelectedDeck();
            if(!_decksPreviewWindowManager.HasAvailableDecks())
            {
                _selectedDeckManager.Disable();
            }
        }

        private void OnEndTurnButtonClicked(object sender, EventArgs e)
        {
            float totalCost = 0;

            List<Card> selectedCards = _deckOnHandsManager.GetPickedCards();
            List<Card> markedForDisenchantCards = _deckOnHandsManager.GetCardsMarkedForDisenchant();
            if (selectedCards.Count > 0 || markedForDisenchantCards.Count > 0)
            {
                totalCost = selectedCards.Where(item => item != null).Sum(item => item.Cost);
                totalCost -= markedForDisenchantCards.Where(item => item != null).Sum(item => item.Cost);
                CardsTurnEnd?.Invoke(this, new CardsEndTurnEventArgs(totalCost, selectedCards));
                return;
            }

            List<Ability> selectedAbilities = _abilitiesQueueManager.GetSelectedAbilities();
            MeleeTurnEnd?.Invoke(this, new MeleeEndTurnEventArgs(selectedAbilities));
        }

        private void OnClearButtonClicked(object sender, EventArgs e)
        {
            _abilitiesQueueManager.ClearQueue();
            ClearButtonClicked?.Invoke(this, e);
        }

        #endregion

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_combatUIView), _combatUIView);
            this.CheckFieldValueIsNotNull(nameof(_abilitiesQueueManager), _abilitiesQueueManager);
            this.CheckFieldValueIsNotNull(nameof(_meleeAttackListManager), _meleeAttackListManager);
            this.CheckFieldValueIsNotNull(nameof(_activeBlockManager), _activeBlockManager);
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
            _selectedDeckManager.EmptyDeckClicked -= OnEmptyDeckClicked;
            _selectedDeckManager.SelectedDeckClicked -= OnSelectedDeckClicked;
            _decksPreviewWindowManager.DeckSelected -= OnDeckSelected;
            _deckOnHandsManager.CardSelectClicked -= OnCardSelectClicked;
            _deckOnHandsManager.PickedCardsCountChanged -= _combatUIView.OnSelectedCardsCountChanged;
            _combatUIView.EndTurnButtonClicked -= OnEndTurnButtonClicked;
            _combatUIView.ClearButtonClicked -= OnClearButtonClicked;
        }
    }
}
