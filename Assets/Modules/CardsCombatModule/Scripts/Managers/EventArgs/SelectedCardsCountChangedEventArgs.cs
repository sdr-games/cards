using System;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class SelectedCardsCountChangedEventArgs : EventArgs
    {
        public bool IsEmpty { get; private set; }

        public SelectedCardsCountChangedEventArgs(bool isEmpty)
        {
            IsEmpty = isEmpty;
        }
    }
}