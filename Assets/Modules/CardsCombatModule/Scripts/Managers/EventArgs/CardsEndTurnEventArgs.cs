using System.Collections.Generic;

using SDRGames.Whist.CardsCombatModule.Models;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class CardsEndTurnEventArgs
    {
        public float TotalCost { get; private set; }
        public List<Card> Cards { get; private set; }

        public CardsEndTurnEventArgs(float totalCost, List<Card> cards)
        {
            TotalCost = totalCost;
            Cards = cards;
        }
    }
}