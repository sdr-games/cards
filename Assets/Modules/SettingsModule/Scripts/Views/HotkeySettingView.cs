using System;
using System.Collections;
using System.Collections.Generic;
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

        public event EventHandler OnClick;

        public void Initialize(string caption = "ABC", string currentValue = " ")
        {
            _caption.text = caption;
            _hotkeyValue.text = currentValue;
            _hotkey.onClick.AddListener(Clicked);
        }

        private void Clicked()
        {
            OnClick?.Invoke(this, EventArgs.Empty);
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
            Initialize();
        }
    }
}
