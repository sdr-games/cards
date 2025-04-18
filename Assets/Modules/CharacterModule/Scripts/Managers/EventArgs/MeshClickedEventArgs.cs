using System;

namespace SDRGames.Whist.CharacterModule.Managers
{
    public class MeshClickedEventArgs : EventArgs
    {
        public bool IsSelected { get; private set; }

        public MeshClickedEventArgs(bool isSelected)
        {
            IsSelected = isSelected;
        }
    }
}