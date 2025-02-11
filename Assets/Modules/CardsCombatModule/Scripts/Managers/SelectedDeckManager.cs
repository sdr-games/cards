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

        public event EventHandler EmptyDeckClicked;
        public event EventHandler<SelectedDeckViewClickedEventArgs> SelectedDeckClicked;

        public void Initialize(UserInputController userInputController)
        {
            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;

            _selectedDeckPresenter = new SelectedDeckPresenter(_selectedDeckView);
        }

        public void SetSelectedDeck(Deck deck)
        {
            _deck = deck;
            _selectedDeckPresenter.SetSelectedDeck(_deck);
        }

        public void UnsetSelectedDeck()
        {
            _deck = null;
            _selectedDeckPresenter.UnsetSelectedDeck();
        }

        public override void Hide()
        {
            _visible = false;
            base.Hide();
        }

        public void Disable()
        {
            Hide();
            gameObject.SetActive(false);
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject)
            {
                if (IsEmpty)
                {
                    EmptyDeckClicked?.Invoke(this, EventArgs.Empty);
                    return;
                }
                _visible = !_visible;
                SelectedDeckClicked?.Invoke(this, new SelectedDeckViewClickedEventArgs(_visible));
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
