using System;
using System.Collections.Generic;
using UnityEngine;

using static TMPro.TMP_Dropdown;

namespace SDRGames.Whist.SettingsModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DropdownSetting", menuName = "SDRGames/Settings/Dropdown")]
    public class DropdownSettingScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string Name { get; protected set; }
        [field: SerializeField] public List<OptionData> Values { get; protected set; }
        public int CurrentIndex { get; protected set; }

        public void SetCurrentIndex(int index)
        {
            CurrentIndex = index;
        }
    }
}
