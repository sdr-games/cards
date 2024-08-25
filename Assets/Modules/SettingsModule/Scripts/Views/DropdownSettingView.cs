using System;
using System.Collections.Generic;

using SDRGames.Whist.SettingsModule.Managers;

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

        public event EventHandler<DropdownChangeSettingsEventArgs> OnValueChanged;

        public void Initialize(string caption, List<OptionData> options, int currentIndex = 0)
        {
            _caption.text = caption;
            _dropdown.options = options;
            _dropdown.SetValueWithoutNotify(currentIndex);
            _dropdown.onValueChanged.AddListener(ValueChanged);
        }

        private void ValueChanged(int index)
        {
            OnValueChanged?.Invoke(this, new DropdownChangeSettingsEventArgs(index, _dropdown.options[index].text));
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
        }
    }
}
