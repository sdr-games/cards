using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
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
            Cards = new List<Card>();
            foreach(AbilityWithComboScriptableObject cardSO in deckScriptableObject.Cards)
            {
                Card card = new Card(cardSO);
                Cards.Add(card);
            } 
        }

        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
        }
    }
}
