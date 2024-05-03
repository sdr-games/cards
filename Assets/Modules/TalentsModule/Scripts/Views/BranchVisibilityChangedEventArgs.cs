using System;

namespace SDRGames.Whist.TalentsModule.Views
{
    public class BranchVisibilityChangedEventArgs : EventArgs
    {
        public bool IsVisible { get; private set; }

        public BranchVisibilityChangedEventArgs(bool isVisible)
        {
            IsVisible = isVisible;
        }
    }
}