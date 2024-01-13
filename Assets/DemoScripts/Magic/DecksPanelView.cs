using System;
using System.Collections.Generic;
using UnityEngine;

public class DecksPanelView : MonoBehaviour
{
    [SerializeField] private DeckView _deckViewPrefab;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private DeckPreviewView _deckPreview;

    [SerializeField] private Deck[] _decks;

    private bool _showed;

    public event EventHandler<List<Card>> AttackApplied;

    public void Initialize()
    {
        foreach (Deck deck in _decks)
        {
            DeckView deckView = Instantiate(_deckViewPrefab, transform);
            deckView.Initialize(deck, _deckPreview);
            deckView.CardsShown += OnCardsShown;
        }
        _deckPreview.DeckSelected += OnDeckSelected;
        _deckPreview.PreviewClosed += Show;
    }

    private void OnDeckSelected(object sender, EventArgs e)
    {
        Hide();
    }

    private void Show(object sender, EventArgs e)
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
        _showed = false;
    }

    public void SwitchVisibility()
    {
        if (!_showed)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
        }
        else
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }
        _showed = !_showed;
    }

    public void Hide()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        _showed = false;
    }

    private void OnCardsShown(object sender, EventArgs e)
    {
        _canvasGroup.alpha = 0.5f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
    }
}
