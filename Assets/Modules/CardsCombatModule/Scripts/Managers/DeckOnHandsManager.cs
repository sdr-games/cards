using System;
using System.Collections.Generic;

using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.CardsCombatModule.Views;
using SDRGames.Whist.CardsCombatModule.Models;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class DeckOnHandsManager : MonoBehaviour
    {
        [SerializeField] private DeckOnHandView _deckOnHandView;
        [SerializeField] private int _maxCardsOnHandsCount = 4;

        private Deck _deck;

        private UserInputController _userInputController;
        private List<CardManager> _cards;
        private List<CardManager> _selectedCardsManagers;

        public bool IsEmpty => _selectedCardsManagers.Count == 0;

        public event EventHandler<CardClickedEventArgs> CardClicked;
        public event EventHandler<CardsSelectionClearedEventArgs> CardsSelectionCleared;
        public event EventHandler<SelectedCardsCountChangedEventArgs> SelectedCardsCountChanged;

        public void Initialize(UserInputController userInputController)
        {
            _cards = new List<CardManager>();
            _selectedCardsManagers = new List<CardManager>();

            _userInputController = userInputController;
        }

        public void SetSelectedDeck(DeckScriptableObject deckScriptableObject)
        {
            _deck = new Deck(deckScriptableObject);
            int count = _deck.Cards.Count > _maxCardsOnHandsCount ? _maxCardsOnHandsCount : _deck.Cards.Count;
            for (int i = 0; i < count; i++)
            {
                int index = UnityEngine.Random.Range(0, _deck.Cards.Count - 1);
                CardManager cardManager = _deckOnHandView.DrawCard(count, i);
                cardManager.Initialize(_userInputController, _deck.Cards[index], _deck.Cards.Count - i - 1);
                cardManager.CardClicked += OnCardClicked;
                _cards.Add(cardManager);
            }
        }

        public bool TryAddSelectedCard(CardManager cardManager)
        {
            if (_selectedCardsManagers.Contains(cardManager))
            {
                return false;
            }
            _selectedCardsManagers.Add(cardManager);
            cardManager.Select();
            SelectedCardsCountChanged?.Invoke(this, new SelectedCardsCountChangedEventArgs(_selectedCardsManagers.Count <= 0));
            return true;
        }

        public bool TryDeselectCard(CardManager cardManager)
        {
            if (!_selectedCardsManagers.Contains(cardManager))
            {
                return false;
            }
            _selectedCardsManagers.Remove(cardManager);
            cardManager.Deselect();
            SelectedCardsCountChanged?.Invoke(this, new SelectedCardsCountChangedEventArgs(_selectedCardsManagers.Count <= 0));
            return true;
        }

        public bool TryRemoveSelectedCard(CardManager cardManager)
        {
            if(!TryDeselectCard(cardManager))
            {
                return false;
            }
            cardManager.CardClicked -= OnCardClicked;
            cardManager.Destroy();
            return true;
        }

        public void ClearCardsSelection()
        {
            float reverseAmount = 0;
            List<CardManager> selectedCardsManagers = new List<CardManager>(_selectedCardsManagers);
            foreach (CardManager cardManager in selectedCardsManagers)
            {
                TryDeselectCard(cardManager);
                reverseAmount += cardManager.Card.Cost;
            }
            CardsSelectionCleared?.Invoke(this, new CardsSelectionClearedEventArgs(reverseAmount));
            SelectedCardsCountChanged?.Invoke(this, new SelectedCardsCountChangedEventArgs(_selectedCardsManagers.Count <= 0));
        }

        public List<Card> PopSelectedCards()
        {
            if(_selectedCardsManagers.Count == 0)
            {
                return null;
            }

            List<Card> selectedCards = new List<Card>();
            List<CardManager> selectedCardsManagers = new List<CardManager>(_selectedCardsManagers);
            foreach(CardManager cardManager in selectedCardsManagers)
            {
                selectedCards.Add(cardManager.Card);
                TryRemoveSelectedCard(cardManager);
            }
            return selectedCards;
        }

        public void ShowView()
        {
            _deckOnHandView.Show();
        }

        public void HideView()
        {
            _deckOnHandView.Hide();
            foreach(CardManager cardManager in _selectedCardsManagers)
            {
                cardManager.Deselect();
            }
            SelectedCardsCountChanged?.Invoke(this, new SelectedCardsCountChangedEventArgs(_selectedCardsManagers.Count <= 0));
        }

        private void OnCardClicked(object sender, CardClickedEventArgs e)
        {
            CardClicked?.Invoke(this, new CardClickedEventArgs(e.CardManager, e.IsSelected));
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_deckOnHandView), _deckOnHandView);
        }

        private void OnDestroy()
        {
            foreach(CardManager cardManager in _cards)
            {
                cardManager.CardClicked -= OnCardClicked;
            } 
        }
    }
}
