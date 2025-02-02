using System.Collections.Generic;

using SDRGames.Whist.CardsCombatModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Models
{
    public class Deck
    {
        [field: SerializeField] public Sprite Backside { get; private set; }
        [field: SerializeField] public List<Card> Cards { get; private set; }

        public Deck(DeckScriptableObject deckScriptableObject)
        {
            Backside = deckScriptableObject.Backside;
            foreach(CardScriptableObject cardSO in deckScriptableObject.Cards)
            {
                Card card = new Card(cardSO);
                Cards.Add(card);
            } 
        }
    }
}
