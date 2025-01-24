using System.Collections.Generic;

using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class CardsEndTurnEventArgs
    {
        public float TotalCost { get; private set; }
        public List<CardScriptableObject> Cards { get; private set; }

        public CardsEndTurnEventArgs(float totalCost, List<CardScriptableObject> cards)
        {
            TotalCost = totalCost;
            Cards = cards;
        }
    }
}