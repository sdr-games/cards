using System;

namespace SDRGames.Whist.TalentsModule.Views
{
    public class BlockChangedEventArgs : EventArgs
    {
        public bool IsBlocked { get; private set; }

        public BlockChangedEventArgs(bool isBlocked)
        {
            IsBlocked = isBlocked;
        }
    }
}