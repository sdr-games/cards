using System;

using SDRGames.Whist.CardsCombatModule.Presenters;
using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.CardsCombatModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class DeckPreviewManager : MonoBehaviour
    {
        [SerializeField] private DeckPreviewView _deckPreviewView;

        private DeckScriptableObject _deck;
        private UserInputController _userInputController;

        public event EventHandler<DeckPreviewClickedEventArgs> DeckPreviewClicked;

        public void Initialize(UserInputController userInputController, DeckScriptableObject deck)
        {
            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;

            _deck = deck;

            new DeckPreviewPresenter(_deckPreviewView, deck);
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject)
            {
                DeckPreviewClicked?.Invoke(this, new DeckPreviewClickedEventArgs(_deck));
            }
        }

        private void OnEnable()
        {
            if (_deckPreviewView == null)
            {
                Debug.LogError("Deck Preview View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
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
