using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Deck
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Back { get; private set; }
    [field: SerializeField] public List<Card> Cards { get; private set; }

    public event EventHandler<string> DescriptionUpdated;

    public void RemoveCard(Card card)
    {
        Cards.Remove(card);
        Description = Cards.Count.ToString();
        DescriptionUpdated?.Invoke(this, Description);
    }
}
