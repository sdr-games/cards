using System;
using System.Linq;
using System.Collections.Generic;

using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.CardsCombatModule.Views;
using SDRGames.Whist.CardsCombatModule.Models;

using UnityEngine;
using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class DeckOnHandsManager : MonoBehaviour
    {
        [SerializeField] private DeckOnHandView _deckOnHandView;
        [SerializeField] private int _maxCardsOnHandsCount = 4;

        private Deck _deck;

        private UserInputController _userInputController;
        private List<CardManager> _cards;
        private List<CardManager> _selectedCards;

        public bool IsEmpty => _selectedCards.Count == 0;

        public event EventHandler<CardClickedEventArgs> CardClicked;
        public event EventHandler<SelectedCardsCountChangedEventArgs> SelectedCardsCountChanged;

        public void Initialize(UserInputController userInputController)
        {
            _cards = new List<CardManager>();
            _selectedCards = new List<CardManager>();

            _userInputController = userInputController;
        }

        public void SetSelectedDeck(DeckScriptableObject deckScriptableObject)
        {
            _deck = new Deck(deckScriptableObject.Cards);
            int count = _deck.Cards.Length > _maxCardsOnHandsCount ? _maxCardsOnHandsCount : _deck.Cards.Length;
            for (int i = 0; i < count; i++)
            {
                int index = UnityEngine.Random.Range(0, _deck.Cards.Length - 1);
                CardManager cardManager = _deckOnHandView.DrawCard(count, i);
                cardManager.Initialize(_userInputController, _deck.Cards[index], _deck.Cards.Length - i - 1);
                cardManager.CardClicked += OnCardClicked;
                _cards.Add(cardManager);
            }
        }

        public bool TryAddSelectedCard(CardManager cardManager)
        {
            if (_selectedCards.Contains(cardManager))
            {
                return false;
            }
            _selectedCards.Add(cardManager);
            cardManager.Select();
            SelectedCardsCountChanged?.Invoke(this, new SelectedCardsCountChangedEventArgs(_selectedCards.Count <= 0));
            return true;
        }

        public bool TryDeselectCard(CardManager cardManager)
        {
            if (!_selectedCards.Contains(cardManager))
            {
                return false;
            }
            _selectedCards.Remove(cardManager);
            cardManager.Deselect();
            SelectedCardsCountChanged?.Invoke(this, new SelectedCardsCountChangedEventArgs(_selectedCards.Count <= 0));
            return true;
        }

        public bool TryRemoveSelectedCard(CardManager cardManager)
        {
            if(!TryDeselectCard(cardManager))
            {
                return false;
            }
            cardManager.CardClicked -= OnCardClicked;
            //cardManager.Destroy();
            return true;
        }

        public List<CardScriptableObject> GetSelectedCards()
        {
            if(_selectedCards.Count == 0)
            {
                return null;
            }

            List<CardScriptableObject> selectedCards = new List<CardScriptableObject>();
            foreach(CardManager cardManager in _selectedCards)
            {
                selectedCards.Add(cardManager.CardScriptableObject);
            }
            return selectedCards.OrderBy(card => card.AbilityModifiers.Length).ToList();
        }

        public void ShowView()
        {
            _deckOnHandView.Show();
        }

        public void HideView()
        {
            _deckOnHandView.Hide();
            foreach(CardManager cardManager in _selectedCards)
            {
                cardManager.Deselect();
            }
            SelectedCardsCountChanged?.Invoke(this, new SelectedCardsCountChangedEventArgs(_selectedCards.Count <= 0));
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
