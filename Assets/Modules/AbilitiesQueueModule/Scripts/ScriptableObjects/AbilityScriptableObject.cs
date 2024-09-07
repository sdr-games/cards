using SDRGames.Whist.LocalizationModule.Models;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects
{
    public class AbilityScriptableObject : ScriptableObject
    {
        [field: SerializeField] public LocalizedString Name { get; private set; }
        [field: SerializeField] public LocalizedString Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }

        private void OnEnable()
        {
            if (Name == null)
            {
                Debug.LogError("Name не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (Description == null)
            {
                Debug.LogError("Description не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (Icon == null)
            {
                Debug.LogError("Icon не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            //if (AbilityLogics.Length == 0)
            //{
            //    Debug.LogError("Name не был назначен");
            //    #if UNITY_EDITOR
            //        EditorApplication.isPlaying = false;
            //    #endif
            //}
        }
    }
}
