using System;

namespace SDRGames.Whist.DiceModule
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