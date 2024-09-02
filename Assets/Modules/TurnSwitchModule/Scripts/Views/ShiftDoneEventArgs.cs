using System;

namespace SDRGames.Whist.TurnSwitchModule
{
    public class ShiftDoneEventArgs : EventArgs
    {
        public int CurrentIndex { get; private set; }

        public ShiftDoneEventArgs(int currentIndex)
        {
            CurrentIndex = currentIndex;
        }
    }
}