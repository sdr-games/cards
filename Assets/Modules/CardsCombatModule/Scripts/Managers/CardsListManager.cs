using System.Collections.Generic;

using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.CardsCombatModule.Views;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class CardsListManager : MonoBehaviour
    {
        [SerializeField] private CardPreviewView[] _cardPreviewViews;

        public void Initialize(List<Card> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Card card = cards[i];
                _cardPreviewViews[i].Initialize(card.Name, card.GetLocalizedDescription(), card.Icon);
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
