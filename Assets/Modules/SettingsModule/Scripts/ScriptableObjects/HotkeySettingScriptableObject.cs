using UnityEngine;

namespace SDRGames.Whist.SettingsModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "HotkeySetting", menuName = "SDRGames/Settings/Hotkey")]
    public class HotkeySettingScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public KeyCode DefaultValue { get; private set; }
        [field: SerializeField] public string CurrentValue { get; private set; }

        public void SetCurrentValue(string value)
        {
            CurrentValue = value;
        }

        private void OnEnable()
        {
            if(CurrentValue == "")
            {
                CurrentValue = DefaultValue.ToString();
            }
        }
    }
}
