using System;

namespace SDRGames.Whist.TalentsModule.Models
{
    public class BlockChangedEventArgs : EventArgs
    {
        public bool IsBlocked { get; private set; }

        public BlockChangedEventArgs(bool isActive)
        {
            IsBlocked = isActive;
        }
    }
}