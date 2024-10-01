using System;

using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class DecksPreviewWindowManager : HideableUIView
    {
        [SerializeField] private DeckScriptableObject[] _decks;
        [SerializeField] private CardsListManager _cardsListManager;
        [SerializeField] private DecksListManager _decksListManager;
        [SerializeField] private Button _selectButton;

        private UserInputController _userInputController;

        public event EventHandler<DeckPreviewClickedEventArgs> DeckSelected;
 
        public void Initialize(UserInputController userInputController)
        {
            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
            _decksListManager.Initialize(_userInputController, _cardsListManager, _decks);
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if(e.GameObject == _selectButton.gameObject)
            {
                DeckSelected?.Invoke(this, new DeckPreviewClickedEventArgs(_decksListManager.SelectedDeck));
                Hide();
            }
        }

        public override void Show()
        {
            _decksListManager.ResetSelection();
            base.Show();
        }

        private void OnEnable()
        {
            if (_cardsListManager == null)
            {
                Debug.LogError("Cards List Manager не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_decksListManager == null)
            {
                Debug.LogError("Deck List Manager не были назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_decks.Length == 0)
            {
                Debug.LogError("Decks не были назначены");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_selectButton == null)
            {
                Debug.LogError("Select Button не была назначена ");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }

        private void OnDisable()
        {
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
        }
    }
}
