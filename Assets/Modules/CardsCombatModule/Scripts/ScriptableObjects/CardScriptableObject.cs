using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CardScriptableObject", menuName = "SDRGames/Combat/Cards/Card")]
    public class CardScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public Sprite Illustration { get; private set; }
        [field: SerializeField] public ScriptableObject[] AbilityLogics { get; private set; }

        private void OnEnable()
        {
            if (Name == string.Empty)
            {
                Debug.LogError("Name не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (Description == string.Empty)
            {
                Debug.LogError("Description не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (Illustration == null)
            {
                Debug.LogError("Illustration не был назначен");
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
