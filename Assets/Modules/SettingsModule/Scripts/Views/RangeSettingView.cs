using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.SettingsModule.Views
{
    public class RangeSettingView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _caption;
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _currentValue;

        public event EventHandler<float> OnValueChanged;

        public void Initialize(string caption = "ABC", float currentValue = 0, float maxValue = 100)
        {
            _caption.text = caption;
            _slider.maxValue = maxValue;
            _slider.value = currentValue;
            _currentValue.text = currentValue.ToString();
            _slider.onValueChanged.AddListener(ValueChanged);
        }

        private void ValueChanged(float value)
        {
            _currentValue.text = value.ToString();
            OnValueChanged?.Invoke(this, value);
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

            if (_slider == null)
            {
                Debug.LogError("Slider не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_currentValue == null)
            {
                Debug.LogError("Current Value не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
            Initialize();
        }
    }
}
