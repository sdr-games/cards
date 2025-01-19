using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Models
{
    public class Deck
    {
        [field: SerializeField] public CardScriptableObject[] Cards { get; private set; }

        public Deck(CardScriptableObject[] cards)
        {
            Cards = cards;
        }
    }
}
