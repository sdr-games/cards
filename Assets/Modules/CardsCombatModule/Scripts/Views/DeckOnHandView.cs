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

        public CardManager DrawCard(int maxCount, int index)
        {
            CardManager cardManager = Instantiate(_cardManagerPrefab, transform, false);
            RedrawCard(cardManager, maxCount, index);
            return cardManager;
        }

        public void RedrawCard(CardManager cardManager, int maxCount, int index)
        {
            Vector2 position = CalculatePosition(maxCount, index);
            cardManager.UpdateView(position, index);
        }

        private Vector2 CalculatePosition(int maxCount, int currentIndex)
        {
            float radiansOfSeparation = Mathf.PI / 2 / maxCount * (currentIndex + 0.5f);
            return new Vector2(Mathf.Cos(radiansOfSeparation) * _rectTransform.sizeDelta.x, Mathf.Sin(radiansOfSeparation) * _rectTransform.sizeDelta.x);
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_rectTransform), _rectTransform);
            this.CheckFieldValueIsNotNull(nameof(_cardManagerPrefab), _cardManagerPrefab);
        }
    }
}
