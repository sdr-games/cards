using System;

namespace SDRGames.Whist.GlobalMapModule.Views
{
    public class PinClickedEventArgs : EventArgs
    {
        public GlobalMapPinView.Status Status { get; private set; }

        public PinClickedEventArgs(GlobalMapPinView.Status status)
        {
            Status = status;
        }
    }
}