using SDRGames.Whist.SettingsModule.ScriptableObjects;
using SDRGames.Whist.SettingsModule.Views;

using UnityEngine;
using UnityEngine.Events;

namespace SDRGames.Whist.SettingsModule.Managers
{
    public class RangeSettingManager : MonoBehaviour
    {
        [SerializeField] private RangeSettingScriptableObject _rangeSettingModel;
        [SerializeField] private RangeSettingView _rangeSettingView;
        [SerializeField] private UnityEvent<RangeChangeSettingsEventArgs> _updateSettingEvent;

        public void Initialize(RangeSettingScriptableObject rangeSettingModel)
        {
            _rangeSettingView.OnValueChanged += ChangeSetting;
            _rangeSettingView.Initialize(rangeSettingModel.Name, rangeSettingModel.CurrentValue, rangeSettingModel.MinValue, rangeSettingModel.MaxValue);
        }

        private void ChangeSetting(object sender, RangeChangeSettingsEventArgs e)
        {
            _rangeSettingModel.SetCurrentValue(e.Value);
            _updateSettingEvent.Invoke(e);
        }

        private void Start()
        {
            Initialize(_rangeSettingModel);
        }
    }
}
