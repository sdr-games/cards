using System;
using System.Collections.Generic;

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
        [SerializeField] private int _maxCardsInQueue = 3;

        private Deck _deck;

        private UserInputController _userInputController;
        private List<CardManager> _cards;
        private List<CardManager> _pickedCardsManagers;
        private List<CardManager> _markedForDisenchantCardsManagers;

        public bool IsEmpty => _pickedCardsManagers.Count == 0;

        public event EventHandler DeckUnsetted;
        public event EventHandler<CardSelectClickedEventArgs> CardSelectClicked;
        public event EventHandler<CardMarkClickedEventArgs> CardMarkClicked;
        public event EventHandler<CardsSelectionClearedEventArgs> CardsSelectionCleared;
        public event EventHandler<SelectedCardsCountChangedEventArgs> PickedCardsCountChanged;

        public void Initialize(UserInputController userInputController)
        {
            _cards = new List<CardManager>();
            _pickedCardsManagers = new List<CardManager>();
            _markedForDisenchantCardsManagers = new List<CardManager>();

            _userInputController = userInputController;
        }

        public void SetSelectedDeck(Deck deck)
        {
            _deck = deck;
            PullCardsFromDeck();
        }

        public void PullCardsFromDeck()
        {
            if(_deck == null)
            {
                return;
            } 

            int count = _deck.Cards.Count + _cards.Count > _maxCardsOnHandsCount ? _maxCardsOnHandsCount : _deck.Cards.Count + _cards.Count;
            int existedCount = _cards.Count;
            for (int i = existedCount; i < count; i++)
            {
                int index = UnityEngine.Random.Range(0, _deck.Cards.Count - 1);
                CardManager cardManager = _deckOnHandView.DrawCard();
                cardManager.Initialize(_userInputController, _deck.Cards[index]);
                cardManager.CardPicked += OnCardPicked;
                cardManager.CardMarked += OnCardMarked;
                _deck.RemoveCard(cardManager.Card);
                _cards.Add(cardManager);
            }
        }

        public bool TryPickCard(CardManager cardManager)
        {
            if (_pickedCardsManagers.Contains(cardManager) || _pickedCardsManagers.Count >= _maxCardsInQueue)
            {
                return false;
            }
            TryUnmarkCardForDisenchant(cardManager);
            _pickedCardsManagers.Add(cardManager);
            cardManager.Pick();
            PickedCardsCountChanged?.Invoke(this, new SelectedCardsCountChangedEventArgs(_pickedCardsManagers.Count <= 0 || _markedForDisenchantCardsManagers.Count <= 0));
            return true;
        }

        public bool TryCancelPickCard(CardManager cardManager)
        {
            if (!_pickedCardsManagers.Contains(cardManager))
            {
                return false;
            }
            _pickedCardsManagers.Remove(cardManager);
            cardManager.CancelPick();
            PickedCardsCountChanged?.Invoke(this, new SelectedCardsCountChangedEventArgs(_pickedCardsManagers.Count <= 0 || _markedForDisenchantCardsManagers.Count <= 0));
            return true;
        }

        public bool TryMarkCardForDisenchant(CardManager cardManager)
        {
            if (_markedForDisenchantCardsManagers.Contains(cardManager))
            {
                return false;
            }
            TryCancelPickCard(cardManager);
            _markedForDisenchantCardsManagers.Add(cardManager);
            cardManager.MarkForDisenchant();
            PickedCardsCountChanged?.Invoke(this, new SelectedCardsCountChangedEventArgs(_pickedCardsManagers.Count <= 0 || _markedForDisenchantCardsManagers.Count <= 0));
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
            PickedCardsCountChanged?.Invoke(this, new SelectedCardsCountChangedEventArgs(_pickedCardsManagers.Count <= 0 || _markedForDisenchantCardsManagers.Count <= 0));
            return true;
        }

        public bool TryRemovePickedCard(CardManager cardManager)
        {
            if(!TryCancelPickCard(cardManager))
            {
                return false;
            }
            RemoveCard(cardManager);
            cardManager.CardPicked -= OnCardPicked;
            cardManager.Destroy();
            return true;
        }

        public bool TryRemoveMarkedCard(CardManager cardManager)
        {
            if (!TryUnmarkCardForDisenchant(cardManager))
            {
                return false;
            }
            RemoveCard(cardManager);
            cardManager.CardMarked -= OnCardMarked;
            cardManager.Destroy();
            return true;
        }

        public void ClearCardsQueue()
        {
            float reverseAmount = 0;
            List<CardManager> pickedCardsManagers = new List<CardManager>(_pickedCardsManagers);
            foreach (CardManager cardManager in pickedCardsManagers)
            {
                TryCancelPickCard(cardManager);
                reverseAmount += cardManager.Card.Cost;
            }
            List<CardManager> markedForDisenchantCardsManagers = new List<CardManager>(_markedForDisenchantCardsManagers);
            foreach(CardManager cardManager in markedForDisenchantCardsManagers)
            {
                TryUnmarkCardForDisenchant(cardManager);
                reverseAmount -= cardManager.Card.Cost;
            } 
            CardsSelectionCleared?.Invoke(this, new CardsSelectionClearedEventArgs(reverseAmount));
            PickedCardsCountChanged?.Invoke(this, new SelectedCardsCountChangedEventArgs(_pickedCardsManagers.Count <= 0 && _markedForDisenchantCardsManagers.Count <= 0));
        }

        public List<Card> GetPickedCards()
        {
            List<Card> pickedCards = new List<Card>();
            foreach(CardManager cardManager in _pickedCardsManagers)
            {
                pickedCards.Add(cardManager.Card);
            }
            return pickedCards;
        }

        public List<Card> GetCardsMarkedForDisenchant()
        {
            List<Card> markedCards = new List<Card>();
            foreach (CardManager cardManager in _markedForDisenchantCardsManagers)
            {
                markedCards.Add(cardManager.Card);
            } 
            return markedCards;
        }

        public void RemoveUsedCards()
        {
            List<CardManager> pickedCardsManagers = new List<CardManager>(_pickedCardsManagers);
            foreach(CardManager cardManager in pickedCardsManagers)
            {
                TryRemovePickedCard(cardManager);
            }
            List<CardManager> markedCardsManagers = new List<CardManager>(_markedForDisenchantCardsManagers);
            foreach(CardManager cardManager in markedCardsManagers)
            {
                TryRemoveMarkedCard(cardManager);
            }
        }

        public void ShowView()
        {
            _deckOnHandView.Show();
        }

        public void HideView()
        {
            _deckOnHandView.Hide();
            foreach(CardManager cardManager in _pickedCardsManagers)
            {
                cardManager.CancelPick();
                cardManager.UnmarkForDisenchant();
            }
            PickedCardsCountChanged?.Invoke(this, new SelectedCardsCountChangedEventArgs(_pickedCardsManagers.Count <= 0));
        }

        private void RemoveCard(CardManager cardManager)
        {
            _cards.Remove(cardManager);
            if(_cards.Count == 0 && _deck.Cards.Count == 0)
            {
                _deck = null;
                DeckUnsetted?.Invoke(this, EventArgs.Empty);
            } 
        }

        private void OnCardPicked(object sender, CardSelectClickedEventArgs e)
        {
            CardSelectClicked?.Invoke(this, e);
        }

        private void OnCardMarked(object sender, CardMarkClickedEventArgs e)
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
                cardManager.CardPicked -= OnCardPicked;
                cardManager.CardMarked -= OnCardMarked;
            } 
        }
    }
}