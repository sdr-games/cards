using System;

using SDRGames.Whist.CardsCombatModule.Presenters;
using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.CardsCombatModule.Views;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class SelectedDeckManager : HideableUIView
    {
        [SerializeField] private SelectedDeckView _selectedDeckView;

        private UserInputController _userInputController;
        private DeckScriptableObject _deckScriptableObject;
        private SelectedDeckPresenter _selectedDeckPresenter;

        private bool IsEmpty => _deckScriptableObject == null;

        public event EventHandler EmptyDeckViewClicked;
        public event EventHandler SelectedDeckViewClicked;

        public void Initialize(UserInputController userInputController)
        {
            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;

            _selectedDeckPresenter = new SelectedDeckPresenter(_selectedDeckView);
        }

        public void SetSelectedDeck(DeckScriptableObject deckScriptableObject)
        {
            _deckScriptableObject = deckScriptableObject;
            _selectedDeckPresenter.SetSelectedDeck(deckScriptableObject);
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
                SelectedDeckViewClicked?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnEnable()
        {
            if (_selectedDeckView == null)
            {
                Debug.LogError("Selected Deck View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }

        private void OnDisable()
        {
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;    
        }
    }
}
