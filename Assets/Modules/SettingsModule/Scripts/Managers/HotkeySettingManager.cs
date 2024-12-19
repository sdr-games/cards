using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.SettingsModule.ScriptableObjects;
using SDRGames.Whist.SettingsModule.Views;

using UnityEngine;
using UnityEngine.Events;

namespace SDRGames.Whist.SettingsModule.Managers
{
    public class HotkeySettingManager : MonoBehaviour
    {
        [SerializeField] private HotkeySettingScriptableObject _hotkeySettingModel;
        [SerializeField] private HotkeySettingView _hotkeySettingView;
        [SerializeField] private HideableUIView _clickWaiterView;
        [SerializeField] private UnityEvent<HotkeyChangeSettingsEventArgs> _updateSettingEvent;

        public void Initialize(HotkeySettingScriptableObject hotkeySettingModel)
        {
            _hotkeySettingView.OnValueChanged += ChangeSetting;
            _hotkeySettingView.Initialize(hotkeySettingModel.Name, hotkeySettingModel.CurrentValue.ToString(), _clickWaiterView);
        }

        private void ChangeSetting(object sender, HotkeyChangeSettingsEventArgs e)
        {
            _hotkeySettingModel.SetCurrentValue(e.Value);
            _updateSettingEvent.Invoke(e);
        }

        private void Start()
        {
            Initialize(_hotkeySettingModel);
        }
    }
}
