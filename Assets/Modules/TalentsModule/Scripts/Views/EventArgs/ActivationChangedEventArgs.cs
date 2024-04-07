using System;

namespace SDRGames.Whist.TalentsModule.Views
{
    public class ActivationChangedEventArgs : EventArgs
    {
        public bool IsActive { get; private set; }

        public ActivationChangedEventArgs(bool isActive)
        {
            IsActive = isActive;
        }
    }
}