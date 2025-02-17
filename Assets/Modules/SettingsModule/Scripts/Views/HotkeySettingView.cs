using System;
using System.Collections;

using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.SettingsModule.Views
{
    public class HotkeySettingView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _caption;
        [SerializeField] private Button _hotkey;
        [SerializeField] private TextMeshProUGUI _hotkeyValue;

        private HideableUIView _clickWaiterView;
        private bool _waiting = false;

        public event EventHandler<HotkeyChangeSettingsEventArgs> OnValueChanged;

        public void Initialize(string caption, string currentValue, HideableUIView clickWaiterView)
        {
            _caption.text = caption;
            _hotkeyValue.text = currentValue;
            _hotkey.onClick.AddListener(Clicked);
            _clickWaiterView = clickWaiterView;
        }

        private void Clicked()
        {
            if(!_waiting)
            {
                _clickWaiterView.Show();
                StartCoroutine(WaitForKeyPressed());
                return;
            }
        }

        private IEnumerator WaitForKeyPressed()
        {
            UserInputController.AddLastKeyPressedListener();
            while(UserInputController.LastPressedKey == null)
            {
                yield return null;
            }
            _hotkeyValue.text = UserInputController.LastPressedKey;
            _clickWaiterView.Hide();
            OnValueChanged?.Invoke(this, new HotkeyChangeSettingsEventArgs(UserInputController.LastPressedKey));
            UserInputController.RemoveLastPressedKeyListener();
            _waiting = false;
        }

        private void OnEnable()
        {
            if (_caption == null)
            {
                Debug.LogError("Caption не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_hotkey == null)
            {
                Debug.LogError("Hotkey не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_hotkeyValue == null)
            {
                Debug.LogError("Hotkey Value не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }
    }
}
