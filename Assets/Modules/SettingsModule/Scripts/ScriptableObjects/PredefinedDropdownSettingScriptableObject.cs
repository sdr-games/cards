using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

using static TMPro.TMP_Dropdown;

namespace SDRGames.Whist.SettingsModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PredefinedDropdownSetting", menuName = "SDRGames/Settings/Dropdown Predefined")]
    public class PredefinedDropdownSettingScriptableObject : DropdownSettingScriptableObject
    {
        public enum PredefinedOptions { FullscreenModes, Resolutions, RefreshRate, Qualities, Languages }
        [SerializeField] private PredefinedOptions _predefinedOption;

        private void OnEnable()
        {
            Values = new List<OptionData>();
            switch (_predefinedOption)
            {
                case PredefinedOptions.FullscreenModes:
                    foreach(FullScreenMode mode in Enum.GetValues(typeof(FullScreenMode)))
                    {
                        if(mode == FullScreenMode.ExclusiveFullScreen && !SystemInfo.operatingSystem.Contains("Windows"))
                        {
                            continue;
                        }

                        if(mode == FullScreenMode.MaximizedWindow && !SystemInfo.operatingSystem.Contains("Mac"))
                        {
                            continue;
                        }

                        OptionData value = new OptionData() { text = mode.ToString() };
                        Values.Add(value);
                    }
                    break;
                case PredefinedOptions.Resolutions:
                    foreach (Resolution resolution in Screen.resolutions)
                    {
                        if(resolution.width/resolution.height != 4/3 || resolution.width / resolution.height != 16/9 || resolution.width / resolution.height != 16/10 || resolution.width / resolution.height != 21/9)
                        {
                            continue;
                        }
                        OptionData value = new OptionData() { text = $"{resolution.width}x{resolution.height}" };
                        if (Values.Find(item => item.text == value.text) != null)
                        {
                            continue;
                        }
                        Values.Add(value);
                    }
                    break;
                case PredefinedOptions.RefreshRate:
                    foreach (Resolution resolution in Screen.resolutions)
                    {
                        OptionData value = new OptionData() { text = $"{Math.Round(resolution.refreshRateRatio.value)}Hz" };
                        if(Values.Find(item => item.text == value.text) != null)
                        {
                            continue;
                        }
                        Values.Add(value);
                    }
                    break;
                case PredefinedOptions.Qualities:
                    foreach(string quality in QualitySettings.names)
                    {
                        OptionData value = new OptionData() { text = quality };
                        if (Values.Find(item => item.text == value.text) != null)
                        {
                            continue;
                        }
                        Values.Add(value);
                    }
                    //OptionData customValue = new OptionData() { text = "Custom" };
                    //if (Values.Find(item => item.text == customValue.text) != null)
                    //{
                    //    return;
                    //}
                    //Values.Add(customValue);
                    break;
                case PredefinedOptions.Languages:
                    foreach(Locale locale in LocalizationSettings.AvailableLocales.Locales)
                    {
                        OptionData value = new OptionData() { text = locale.Identifier.CultureInfo.NativeName };
                        if (Values.Find(item => item.text == value.text) != null)
                        {
                            continue;
                        }
                        Values.Add(value);
                    }
                    break;
                default:
                    break;
            } 
        }
    }
}
