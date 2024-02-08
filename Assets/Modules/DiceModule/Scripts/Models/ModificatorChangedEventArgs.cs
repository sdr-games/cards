using System;

namespace SDRGames.Islands.DiceModule
{
    public class ModificatorChangedEventArgs : EventArgs
    {
        public int Modificator { get; private set; }

        public ModificatorChangedEventArgs(int modificator)
        {
            Modificator = modificator;
        }
    }
}