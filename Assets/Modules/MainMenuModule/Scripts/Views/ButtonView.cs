using System;

using SDRGames.Whist.MainMenuModule.Managers;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDRGames.Whist.MainMenuModule.Views
{
    public class ButtonView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private SelectionArrowsManager _selectionArrowsManager;
        [SerializeField] private Button _button;

        public event EventHandler ButtonClicked;

        public void Activate()
        {
            _button.interactable = true;
        }

        public void Deactivate()
        {
            _button.interactable = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _selectionArrowsManager.Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _selectionArrowsManager.Hide();
        }

        private void OnEnable()
        {
            if (_button == null)
            {
                Debug.LogError("Button не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_text == null)
            {
                Debug.LogError("Text не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }
    }
}
