using System;

using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.HelpersModule;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class DecksPreviewWindowManager : HideableUIView
    {
        [SerializeField] private CardsListManager _cardsListManager;
        [SerializeField] private DecksListManager _decksListManager;
        [SerializeField] private Button _selectButton;

        private DeckScriptableObject[] _deckScriptableObjects;
        private UserInputController _userInputController;

        public event EventHandler<DeckPreviewClickedEventArgs> DeckSelected;
 
        public void Initialize(UserInputController userInputController, DeckScriptableObject[] deckScriptableObjects)
        {
            _deckScriptableObjects = deckScriptableObjects;
            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
            _decksListManager.Initialize(_userInputController, _cardsListManager, _deckScriptableObjects);
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if(e.GameObject == _selectButton.gameObject)
            {
                DeckPreviewManager selectedDeck = _decksListManager.SelectedDeckPreview;
                _decksListManager.RemoveSelectedDeckFromList();
                DeckSelected?.Invoke(this, new DeckPreviewClickedEventArgs(selectedDeck));
                Hide();
            }
        }

        public override void Show()
        {
            _decksListManager.ResetSelection();
            base.Show();
        }

        public bool HasAvailableDecks()
        {
            return _decksListManager.HasAvailableDecks();
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_cardsListManager), _cardsListManager);
            this.CheckFieldValueIsNotNull(nameof(_decksListManager), _decksListManager);
            this.CheckFieldValueIsNotNull(nameof(_selectButton), _selectButton);

            if (_deckScriptableObjects.Length == 0)
            {
                Debug.LogError("Decks не были назначены");
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
