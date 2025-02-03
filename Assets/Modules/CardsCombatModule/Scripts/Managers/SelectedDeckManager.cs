using System;

using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.CardsCombatModule.Presenters;
using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.CardsCombatModule.Views;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class SelectedDeckManager : HideableUIView
    {
        [SerializeField] private SelectedDeckView _selectedDeckView;

        private UserInputController _userInputController;
        private Deck _deck;
        private SelectedDeckPresenter _selectedDeckPresenter;
        private bool _visible = false;

        private bool IsEmpty => _deck == null;

        public event EventHandler EmptyDeckViewClicked;
        public event EventHandler<SelectedDeckViewClickedEventArgs> SelectedDeckViewClicked;

        public void Initialize(UserInputController userInputController)
        {
            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;

            _selectedDeckPresenter = new SelectedDeckPresenter(_selectedDeckView);
        }

        public void SetSelectedDeck(DeckScriptableObject deckScriptableObject)
        {
            _deck = new Deck(deckScriptableObject);
            _selectedDeckPresenter.SetSelectedDeck(_deck);
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject)
            {
                if (IsEmpty)
                {
                    EmptyDeckViewClicked?.Invoke(this, EventArgs.Empty);
                    return;
                }
                _visible = !_visible;
                SelectedDeckViewClicked?.Invoke(this, new SelectedDeckViewClickedEventArgs(_visible));
            }
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_selectedDeckView), _selectedDeckView);
        }

        private void OnDisable()
        {
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;    
        }
    }
}
