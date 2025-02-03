using System;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class SelectedDeckViewClickedEventArgs : EventArgs
    {
        public bool Visible { get; private set; }

        public SelectedDeckViewClickedEventArgs(bool visible)
        {
            Visible = visible;
        }
    }
}