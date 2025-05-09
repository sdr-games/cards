using SDRGames.Whist.CardsCombatModule.Managers;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.HelpersModule.Views;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Views
{
    public class DeckOnHandView : HideableUIView
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CardManager _cardManagerPrefab;

        public CardManager DrawCard()
        {
            CardManager cardManager = Instantiate(_cardManagerPrefab, _rectTransform, false);
            return cardManager;
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_rectTransform), _rectTransform);
            this.CheckFieldValueIsNotNull(nameof(_cardManagerPrefab), _cardManagerPrefab);
        }
    }
}
