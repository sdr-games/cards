using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.CardsCombatModule.Views;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class CardsListManager : MonoBehaviour
    {
        [SerializeField] private CardPreviewView[] _cardPreviewViews;

        public void Initialize(CardScriptableObject[] cards)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                CardScriptableObject card = cards[i];
                _cardPreviewViews[i].Initialize(card.Name, card.Description, card.Icon);
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
