using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DeckScriptableObject", menuName = "SDRGames/Combat/Cards/Deck")]
    public class DeckScriptableObject : ScriptableObject
    {
        [field: SerializeField] public Sprite Backside { get; private set; }
        [field: SerializeField] public CardScriptableObject[] Cards { get; private set; }

        private void OnEnable()
        {
            if (Backside == null)
            {
                Debug.LogError("Backside не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if(Cards.Length < 16)
            {
                Debug.LogError("Количество Cards должно быть равно 16");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}
