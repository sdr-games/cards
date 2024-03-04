using System;

namespace SDRGames.Whist.DiceModule
{
    public class SidesCountChangedEventArgs : EventArgs
    {
        public int SidesCount { get; private set; }

        public SidesCountChangedEventArgs(int sidesCount)
        {
            SidesCount = sidesCount;
        }
    }
}