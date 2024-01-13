using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckPreviewView : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private List<CardView> _cardViews;
    [SerializeField] private SelectedDeckView _selectedDeckView;

    private Deck _deck;

    public event EventHandler DeckSelected;
    public event EventHandler PreviewClosed;

    public void Initialize(Deck deck)
    {
        _deck = deck;
        List<Card> cards = _deck.Cards;
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];
            _cardViews[i].Initialize(card);
        }
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    public void SelectShownDeck()
    {
        ClosePanel();
        _selectedDeckView.Initialize(_deck);
        DeckSelected?.Invoke(this, EventArgs.Empty);
    }

    public void CloseShownDeck()
    {
        ClosePanel();
        PreviewClosed?.Invoke(this, EventArgs.Empty);
    }

    private void ClosePanel()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
}
