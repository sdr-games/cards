using UnityEngine;

namespace SDRGames.Whist.SettingsModule.Models
{
    [CreateAssetMenu(fileName = "RangeSetting", menuName = "SDRGames/Settings/Range")]
    public class RangeSettingModel : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int MinValue { get; private set; }
        [field: SerializeField] public int MaxValue { get; private set; }
        [field: SerializeField] public float CurrentValue { get; private set; }

        public void SetCurrentValue(float value)
        {
            CurrentValue = value;
        }
    }
}
