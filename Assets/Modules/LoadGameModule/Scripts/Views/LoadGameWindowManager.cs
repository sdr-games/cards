using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.LoadGameModule.Managers
{
    public class LoadGameWindowManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private ButtonView _closeButton;

        [SerializeField] private UserInputController _userInputController;

        public void Initialize(UserInputController userInputController)
        {
            _userInputController = userInputController;
            _closeButton.Initialize(_userInputController, true);
            _closeButton.ButtonClicked += Hide;
        }

        public void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        private void Hide(object sender, System.EventArgs e)
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        private void OnEnable()
        {
            if (_canvasGroup == null)
            {
                Debug.LogError("Canvas Group не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_closeButton == null)
            {
                Debug.LogError("Close Button не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
            Initialize(_userInputController);
        }
    }
}
