using SDRGames.Whist.HelpersModule;

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
            this.CheckFieldValueIsNotNull(nameof(Backside), Backside);

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
