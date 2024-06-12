using SDRGames.Whist.CardsCombatModule.Views;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class CardsListManager : MonoBehaviour
    {
        [SerializeField] private CardPreviewView[] _cardPreviewViews;

        public void Initialize(ScriptableObject[] cards)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                _cardPreviewViews[i].Initialize(null, ""); //probably cards[i].sprite, cards[i].text
            }
        }

        private void OnEnable()
        {
            if (_cardPreviewViews.Length == 0)
            {
                Debug.LogError("Card Preview Views не были назначены");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}
