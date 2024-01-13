using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    public void Initialize(Card card)
    {
        _nameText.text = card.Name;
        _descriptionText.text = card.Description;
    }
}
