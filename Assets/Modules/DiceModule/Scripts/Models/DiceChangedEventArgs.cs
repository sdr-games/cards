using System;

namespace SDRGames.Islands.DiceModule.Models
{
    public class DiceChangedEventArgs : EventArgs
    {
        public Dice Dice { get; private set; }

        public DiceChangedEventArgs(Dice dice)
        {
            Dice = dice;
        }

    }
}