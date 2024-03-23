using System;

using SDRGames.Whist.UserInputModule.Controller;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.ChronotopMapModule.Views
{
    public class ChronotopMapPinModalView : MonoBehaviour
    {
        [SerializeField] private Image _enemyPortrait;
        [SerializeField] private TextMeshProUGUI _enemyName;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Button _fightButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private CanvasGroup _canvasGroup;

        private UserInputController _userInputController;

        public event EventHandler FightButtonClicked;
        public event EventHandler ViewClosed;

        public void Initialize(Sprite enemyPortrait, string enemyName, string description, UserInputController userInputController)
        {
            _enemyPortrait.sprite = enemyPortrait;
            _enemyName.text = enemyName;
            _description.text = description;
            _userInputController = userInputController;
        }

        public void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickOnUI;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickOnUI;
        }

        private void OnLeftMouseButtonClickOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if(e.GameObject == _fightButton.gameObject)
            {
                FightButtonClicked?.Invoke(this, EventArgs.Empty);
            } 
            else
            {
                Hide();
                ViewClosed?.Invoke(this, EventArgs.Empty);
            } 
        }

        private void OnEnable()
        {
            if (_enemyPortrait == null)
            {
                Debug.LogError("Enemy Portrait не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_enemyName == null)
            {
                Debug.LogError("Enemy Name не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_description == null)
            {
                Debug.LogError("Description не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_fightButton == null)
            {
                Debug.LogError("Fight Button не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_closeButton == null)
            {
                Debug.LogError("Close Button не был назначен");
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
        }

        private void OnDisable()
        {
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickOnUI;
        }
    }
}
