using System;

namespace SDRGames.Whist.ActiveBlockModule.Views
{
    public class StanceSwitchedEventArgs : EventArgs
    {
        public bool DefensiveStanceActive { get; private set; }

        public StanceSwitchedEventArgs(bool defensiveStanceActive)
        {
            DefensiveStanceActive = defensiveStanceActive;
        }
    }
}