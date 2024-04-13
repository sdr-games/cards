using System;

namespace SDRGames.Whist.TalentsModule.Models
{
    public class ActiveChangedEventArgs : EventArgs
    {
        public bool IsActive { get; private set; }

        public ActiveChangedEventArgs(bool isActive)
        {
            IsActive = isActive;
        }
    }
}