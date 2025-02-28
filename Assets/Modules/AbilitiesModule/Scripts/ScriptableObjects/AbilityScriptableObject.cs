using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.SoundModule.ScriptableObjects;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesModule.ScriptableObjects
{
    public class AbilityScriptableObject : ScriptableObject
    {
        [field: SerializeField] public LocalizedString Name { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }
        [field: SerializeField] public AbilityLogicScriptableObject[] AbilityLogics { get; private set; }
        [field: SerializeField] public AnimationClip AnimationClip { get; private set; }
        [field: SerializeField] public SoundClipScriptableObject SoundClip { get; private set; }

        public float GetAverageDamage()
        {
            int totalDamage = 0;
            foreach(DamageLogicScriptableObject damageLogicScriptableObject in AbilityLogics)
            {
                totalDamage += damageLogicScriptableObject.DamageValue;
            }
            return totalDamage / AbilityLogics.Length;
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(Name), Name);
            this.CheckFieldValueIsNotNull(nameof(Icon), Icon);
            this.CheckFieldValueIsNotNull(nameof(AnimationClip), AnimationClip);
            //this.CheckFieldValueIsNotNull(nameof(SoundClip), SoundClip);

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
