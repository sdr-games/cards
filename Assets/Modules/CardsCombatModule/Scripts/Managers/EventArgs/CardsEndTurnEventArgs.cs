using System.Collections.Generic;

using SDRGames.Whist.CardsCombatModule.Models;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class CardsEndTurnEventArgs
    {
        public float TotalCost { get; private set; }
        public List<Card> SelectedCards { get; private set; }

        public CardsEndTurnEventArgs(float totalCost, List<Card> selectedCards)
        {
            TotalCost = totalCost;
            SelectedCards = selectedCards;
        }
    }
}