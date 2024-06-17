using System.Collections;
using System.Collections.Generic;
using SDRGames.Whist.CardsCombatModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class DeckOnHandsManager : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CardManager _cardManagerPrefab;

        [SerializeField] private DeckScriptableObject _deck;

        public void Initialize(DeckScriptableObject deck)
        {
            _deck = deck;
            for(int i = 0; i < _deck.Cards.Length; i++)
            {
                Vector2 position = CalculatePositionInRadius(i);
                CardManager cardManager = Instantiate(_cardManagerPrefab, transform, false);
                cardManager.Initialize(position, _deck.Cards[i]);
            }
        }

        private Vector2 CalculatePositionInRadius(int index)
        {
            float radiansOfSeparation = Mathf.PI / 2 / _deck.Cards.Length * (index + 0.5f);
            return new Vector2(Mathf.Cos(radiansOfSeparation) * _rectTransform.sizeDelta.x, Mathf.Sin(radiansOfSeparation) * _rectTransform.sizeDelta.x);
        }

        private void Start()
        {
            Initialize(_deck);
        }
    }
}
