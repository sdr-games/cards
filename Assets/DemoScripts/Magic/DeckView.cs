using System;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DeckView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _back;
    [SerializeField] private CanvasGroup _tooltipCanvasGroup;
    [SerializeField] private TextMeshProUGUI _tooltipName;
    [SerializeField] private TextMeshProUGUI _tooltipDescription;
    
    private DeckPreviewView _deckPreview;

    private Deck _deck;

    public event EventHandler CardsShown;
    public event EventHandler<Deck> DeckSelected;

    public void Initialize(Deck deck, DeckPreviewView deckPreviewView)
    {
        _back.sprite = deck.Back;
        _tooltipName.text = deck.Name;
        _tooltipDescription.text = deck.Description;
        _deck = deck;
        _deckPreview = deckPreviewView;
    }

    public void SetNewTooltipText(string text)
    {
        _tooltipDescription.text = text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_tooltipName != null && _tooltipName.text != "" && _tooltipDescription != null && _tooltipDescription.text != "")
        {
            _tooltipCanvasGroup.alpha = 1;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_tooltipName != null && _tooltipName.text != "" && _tooltipDescription != null && _tooltipDescription.text != "")
        {
            _tooltipCanvasGroup.alpha = 0;
        }
    }

    public void ShowCards()
    {
        _deckPreview.Initialize(_deck);
        CardsShown?.Invoke(this, EventArgs.Empty);
    }
}
