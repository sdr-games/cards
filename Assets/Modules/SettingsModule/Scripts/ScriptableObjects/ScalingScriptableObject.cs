using SDRGames.Whist.SettingsModule.Models;

using UnityEngine;

namespace SDRGames.Whist.SettingsModule.ScriptableObjects
{
    public class ScalingScriptableObject : ScriptableObject
    {
        [SerializeField] private Scaling _scaling;

        private void OnEnable()
        {
            _scaling.UpdateStaticFields();
        }

        private void OnValidate()
        {
            _scaling.UpdateStaticFields();
        }
    }
}
