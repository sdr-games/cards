using SDRGames.Whist.SettingsModule.Models;
using SDRGames.Whist.SettingsModule.Views;

using UnityEngine;
using UnityEngine.Events;

namespace SDRGames.Whist.SettingsModule.Managers
{
    public class DropdownSettingManager : MonoBehaviour
    {
        [SerializeField] private DropdownSettingModel _dropdownSettingModel;
        [SerializeField] private DropdownSettingView _dropdownSettingView;
        [SerializeField] private UnityEvent<DropdownChangeSettingsEventArgs> _updateSettingEvent;

        public void Initialize(DropdownSettingModel dropdownSettingModel)
        {
            _dropdownSettingView.Initialize(dropdownSettingModel.Name, dropdownSettingModel.Values, dropdownSettingModel.CurrentIndex);
            _dropdownSettingView.OnValueChanged += ChangeSetting;
        }

        private void ChangeSetting(object sender, DropdownChangeSettingsEventArgs e)
        {
            _dropdownSettingModel.SetCurrentIndex(e.Index);
            _updateSettingEvent.Invoke(e);
        }

        private void OnEnable()
        {
            Initialize(_dropdownSettingModel);
        }
    }
}
