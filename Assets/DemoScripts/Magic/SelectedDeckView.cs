using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectedDeckView : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private Image _back;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private HandsView _handsView;

    private Deck _deck;

    public void Initialize(Deck deck)
    {
        enabled = true;
        _deck = deck;
        _back.sprite = deck.Back;
        Show();
    }

    public void ShowHands()
    {
        _handsView.Initialize(_deck);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!enabled)
        {
            return;
        }
    }

    private void Show()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
    }

    
}
