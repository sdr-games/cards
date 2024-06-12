using System;

using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class DecksPreviewWindowManager : MonoBehaviour
    {
        [SerializeField] private DeckScriptableObject[] _decks;
        [SerializeField] private CardsListManager _cardsListManager;
        [SerializeField] private DecksListManager _decksListManager;
        [SerializeField] private CanvasGroup _canvasGroup;
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

        public void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
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

            if (_canvasGroup == null)
            {
                Debug.LogError("Canvas Group не был назначен");
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
