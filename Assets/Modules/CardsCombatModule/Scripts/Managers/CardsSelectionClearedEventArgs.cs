using System;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class CardsSelectionClearedEventArgs : EventArgs
    {
        public float ReverseAmount { get; private set; }

        public CardsSelectionClearedEventArgs(float reverseAmount)
        {
            ReverseAmount = reverseAmount;
        }
    }
}