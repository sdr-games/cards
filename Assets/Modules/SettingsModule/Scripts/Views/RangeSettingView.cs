using System;
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

        public event EventHandler<RangeChangeSettingsEventArgs> OnValueChanged;

        public void Initialize(string caption, float currentValue, float minValue, float maxValue)
        {
            _caption.text = caption;
            _slider.minValue = minValue;
            _slider.maxValue = maxValue;
            _slider.onValueChanged.AddListener(ValueChanged);
            _slider.value = currentValue;
            _currentValue.text = Mathf.RoundToInt(currentValue + Math.Abs(minValue) * 2.5f).ToString();
        }

        private void ValueChanged(float value)
        {
            _currentValue.text = Mathf.RoundToInt((value + 40) * 2.5f).ToString();
            OnValueChanged?.Invoke(this, new RangeChangeSettingsEventArgs(value));
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
        }
    }
}
