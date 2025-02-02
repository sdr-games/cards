using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.LocalizationModule.Models;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesModule.ScriptableObjects
{
    public class AbilityScriptableObject : ScriptableObject
    {
        [field: SerializeField] public LocalizedString Name { get; private set; }
        [field: SerializeField] public LocalizedString Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }
        [field: SerializeField] public AbilityLogicScriptableObject[] AbilityLogics { get; private set; }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(Name), Name);
            this.CheckFieldValueIsNotNull(nameof(Description), Description);
            this.CheckFieldValueIsNotNull(nameof(Icon), Icon);

            if (AbilityLogics.Length == 0)
            {
                Debug.LogError($"Ability Logics не был назначен у {name}");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}
