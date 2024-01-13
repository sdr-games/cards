using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsView : MonoBehaviour
{
    [SerializeField] private Deck _deck;
    [SerializeField] private int _cardsCount;

    private List<Card> _selectedCards;

    public void Initialize(Deck deck)
    {
        _deck = deck;
        SelectCards();
    }

    private void SelectCards()
    {
        _selectedCards = new List<Card>();
        for (int i = 0; i < _cardsCount; i++)
        {
            int index = UnityEngine.Random.Range(0, _deck.Cards.Count - 1);
            _selectedCards.Add(_deck.Cards[index]);
        }
    }
}
