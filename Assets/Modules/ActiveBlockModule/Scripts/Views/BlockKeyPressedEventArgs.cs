using System;

namespace SDRGames.Whist.ActiveBlockModule.Views
{
    public class BlockKeyPressedEventArgs : EventArgs
    {
        public bool Correct { get; private set; }

        public BlockKeyPressedEventArgs(bool correct)
        {
            Correct = correct;
        }
    }
}