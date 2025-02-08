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
        private List<CardManager> _markedForDisenchantCardsManagers;

        public bool IsEmpty => _selectedCardsManagers.Count == 0;

        public event EventHandler<CardSelectClickedEventArgs> CardSelectClicked;
        public event EventHandler<CardMarkClickedEventArgs> CardMarkClicked;
        public event EventHandler<CardsSelectionClearedEventArgs> CardsSelectionCleared;
        public event EventHandler<SelectedCardsCountChangedEventArgs> SelectedCardsCountChanged;

        public void Initialize(UserInputController userInputController)
        {
            _cards = new List<CardManager>();
            _selectedCardsManagers = new List<CardManager>();
            _markedForDisenchantCardsManagers = new List<CardManager>();

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
                cardManager.CardSelectClicked += OnCardSelectClicked;
                cardManager.CardMarkClicked += OnCardMarkClicked;
                _cards.Add(cardManager);
            }
        }

        public bool TryAddSelectedCard(CardManager cardManager)
        {
            if (_selectedCardsManagers.Contains(cardManager))
            {
                return false;
            }
            TryUnmarkCardForDisenchant(cardManager);
            _selectedCardsManagers.Add(cardManager);
            cardManager.Select();
            SelectedCardsCountChanged?.Invoke(this, new SelectedCardsCountChangedEventArgs(_selectedCardsManagers.Count <= 0 || _markedForDisenchantCardsManagers.Count <= 0));
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
            SelectedCardsCountChanged?.Invoke(this, new SelectedCardsCountChangedEventArgs(_selectedCardsManagers.Count <= 0 || _markedForDisenchantCardsManagers.Count <= 0));
            return true;
        }

        public bool TryMarkCardForDisenchant(CardManager cardManager)
        {
            if (_markedForDisenchantCardsManagers.Contains(cardManager))
            {
                return false;
            }
            TryDeselectCard(cardManager);
            _markedForDisenchantCardsManagers.Add(cardManager);
            cardManager.MarkForDisenchant();
            SelectedCardsCountChanged?.Invoke(this, new SelectedCardsCountChangedEventArgs(_selectedCardsManagers.Count <= 0 || _markedForDisenchantCardsManagers.Count <= 0));
            return true;
        }

        public bool TryUnmarkCardForDisenchant(CardManager cardManager)
        {
            if (!_markedForDisenchantCardsManagers.Contains(cardManager))
            {
                return false;
            }
            _markedForDisenchantCardsManagers.Remove(cardManager);
            cardManager.UnmarkForDisenchant();
            SelectedCardsCountChanged?.Invoke(this, new SelectedCardsCountChangedEventArgs(_selectedCardsManagers.Count <= 0 || _markedForDisenchantCardsManagers.Count <= 0));
            return true;
        }

        public bool TryRemoveSelectedCard(CardManager cardManager)
        {
            if(!TryDeselectCard(cardManager))
            {
                return false;
            }
            cardManager.CardSelectClicked -= OnCardSelectClicked;
            cardManager.Destroy();
            return true;
        }

        public bool TryRemoveMarkedCard(CardManager cardManager)
        {
            if (!TryUnmarkCardForDisenchant(cardManager))
            {
                return false;
            }
            cardManager.CardMarkClicked -= OnCardMarkClicked;
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
            List<Card> selectedCards = new List<Card>();
            List<CardManager> selectedCardsManagers = new List<CardManager>(_selectedCardsManagers);
            foreach(CardManager cardManager in selectedCardsManagers)
            {
                selectedCards.Add(cardManager.Card);
                TryRemoveSelectedCard(cardManager);
            }
            return selectedCards;
        }

        public List<Card> PopCardsForDisenchant()
        {
            List<Card> markedCards = new List<Card>();
            List<CardManager> markedCardsManagers = new List<CardManager>(_markedForDisenchantCardsManagers);
            foreach (CardManager cardManager in markedCardsManagers)
            {
                markedCards.Add(cardManager.Card);
                TryRemoveMarkedCard(cardManager);
            } 
            return markedCards;
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

        private void OnCardSelectClicked(object sender, CardSelectClickedEventArgs e)
        {
            CardSelectClicked?.Invoke(this, e);
        }

        private void OnCardMarkClicked(object sender, CardMarkClickedEventArgs e)
        {
            CardMarkClicked?.Invoke(this, e);
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_deckOnHandView), _deckOnHandView);
        }

        private void OnDestroy()
        {
            foreach(CardManager cardManager in _cards)
            {
                cardManager.CardSelectClicked -= OnCardSelectClicked;
                cardManager.CardMarkClicked -= OnCardMarkClicked;
            } 
        }
    }
}
