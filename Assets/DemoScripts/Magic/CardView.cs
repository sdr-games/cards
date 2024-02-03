using System;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CardView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    private Card _card;
    private bool _selected;

    public event EventHandler<Card> CardSelected;

    public void Initialize(Card card)
    {
        _card = card;
        _nameText.text = card.Name;
        _descriptionText.text = card.Description;
    }

    public void Select()
    {
        _selected = !_selected;
        CardSelected.Invoke(this, _card);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_selected)
        {
            transform.Translate(Vector3.up * 25);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_selected)
        {
            transform.Translate(Vector3.down * 25);
        }
    }
}
