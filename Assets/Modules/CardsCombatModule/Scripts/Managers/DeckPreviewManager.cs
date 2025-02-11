using System;

using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.CardsCombatModule.Presenters;
using SDRGames.Whist.CardsCombatModule.Views;
using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class DeckPreviewManager : MonoBehaviour
    {
        [SerializeField] private DeckPreviewView _deckPreviewView;
        private UserInputController _userInputController;

        public Deck Deck { get; private set; }

        public event EventHandler<DeckPreviewClickedEventArgs> DeckPreviewClicked;

        public void Initialize(UserInputController userInputController, Deck deck)
        {
            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;

            Deck = deck;

            new DeckPreviewPresenter(_deckPreviewView, deck);
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject)
            {
                DeckPreviewClicked?.Invoke(this, new DeckPreviewClickedEventArgs(this));
            }
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_deckPreviewView), _deckPreviewView);
        }

        private void OnDisable()
        {
            if (_userInputController != null)
            {
                _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
            }
        }
    }
}
