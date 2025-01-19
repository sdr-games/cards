using System.Collections.Generic;
using System.Linq;
using System;

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
        private List<CardManager> _selectedCards;

        public event EventHandler<CardClickedEventArgs> CardClicked;

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

        public void AddSelectedCard(CardManager cardManager)
        {
            if (_selectedCards.Contains(cardManager))
            {
                return;
            }
            _selectedCards.Add(cardManager);
            cardManager.Select();
            SwitchButtonsActivity();
        }

        public bool RemoveSelectedCard(CardManager cardManager)
        {
            if (!_selectedCards.Contains(cardManager))
            {
                return false;
            }
            _selectedCards.Remove(cardManager);
            //cardManager.Destroy();
            SwitchButtonsActivity();
            return true;
        }

        public void ShowView()
        {
            _deckOnHandView.Show();
        }

        public void HideView()
        {
            _deckOnHandView.Hide();
        }

        private void OnApplyButtonClicked(object sender, EventArgs e)
        {
            List<CardManager> selectedCards = new List<CardManager>(_selectedCards).OrderBy(cardManager => cardManager.CardScriptableObject.AbilityModifiers.Length).ToList();
            float totalCost = _selectedCards.Where(item => item != null).Sum(item => item.CardScriptableObject.Cost);
            //ApplyButtonClicked?.Invoke(this, new ApplyButtonClickedEventArgs(totalCost, selectedCards));
        }

        private void OnCardClicked(object sender, CardClickedEventArgs e)
        {
            CardClicked?.Invoke(this, new CardClickedEventArgs(e.CardManager, e.IsSelected));
        }

        private void SwitchButtonsActivity()
        {
            //if (_selectedCards.FirstOrDefault(item => item != null))
            //{
            //    _applyButton.Activate();
            //    return;
            //}
            //_applyButton.Deactivate();
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_deckOnHandView), _deckOnHandView);
        }
    }
}
