using System.Collections.Generic;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class ApplyButtonClickedEventArgs
    {
        public float TotalCost { get; private set; }
        public List<CardManager> Cards { get; private set; }

        public ApplyButtonClickedEventArgs(float totalCost, List<CardManager> cards)
        {
            TotalCost = totalCost;
            Cards = cards;
        }
    }
}