using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsView : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private List<CardView> _selectedCardsViews;
    [SerializeField] private CanvasGroup _applyButton;
    [SerializeField] private CanvasGroup _cancelButton;
    
    private Deck _deck;
    [SerializeField] private int _cardsCount;
    private List<Card> _shownCards;
    private List<Card> _selectedCards;

    public event EventHandler<List<Card>> CardsApplied;

    public void Initialize(Deck deck)
    {
        _deck = deck;
        _selectedCards = new List<Card>();
        SelectCards();
        Show();
    }

    public void ApplyCards()
    {
        CardsApplied?.Invoke(this, _selectedCards);
        SwitchButtonsVisibitily(false);
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }

    private void SelectCards()
    {
        _shownCards = new List<Card>();
        for (int i = 0; i < _cardsCount; i++)
        {
            int index = UnityEngine.Random.Range(0, _deck.Cards.Count - 1);
            _shownCards.Add(_deck.Cards[index]);
            _deck.RemoveCard(_deck.Cards[index]);
        }
    }

    private void Show()
    {
        for(int i = 0; i < _shownCards.Count; i++)
        {
            Card card = _shownCards[i];
            _selectedCardsViews[i].Initialize(card);
            _selectedCardsViews[i].CardSelected += ChangeSelectedCard;
        }
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    private void ChangeSelectedCard(object sender, Card e)
    {
        if (_selectedCards.Contains(e))
        {
            _selectedCards.Remove(e);
        }
        else
        {
            _selectedCards.Add(e);
        }

        bool state = _selectedCards.Count > 0;
        SwitchButtonsVisibitily(state);
    }

    private void SwitchButtonsVisibitily(bool state)
    {
        _applyButton.alpha = state ? 1 : 0;
        _applyButton.interactable = state;
        _applyButton.blocksRaycasts = state;

        _cancelButton.alpha = state ? 1 : 0;
        _cancelButton.interactable = state;
        _cancelButton.blocksRaycasts = state;
    }
}
