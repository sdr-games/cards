using System;
using System.Collections.Generic;

using TMPro;

using UnityEditor;

using UnityEngine;

using static TMPro.TMP_Dropdown;

namespace SDRGames.Whist.SettingsModule.Views
{
    public class DropdownSettingView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _caption;
        [SerializeField] private TMP_Dropdown _dropdown;

        [SerializeField] private List<OptionData> _options;

        public event EventHandler<int> OnValueChanged;

        public void Initialize(string caption = "ABC", List<OptionData> options = null, int currentIndex = 0)
        {
            _caption.text = caption;
            _dropdown.options = _options;
            _dropdown.SetValueWithoutNotify(currentIndex);
            _dropdown.onValueChanged.AddListener(ValueChanged);
        }

        private void ValueChanged(int index)
        {
            OnValueChanged?.Invoke(this, index);
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

            if (_dropdown == null)
            {
                Debug.LogError("Dropdown не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
            Initialize();
        }
    }
}
