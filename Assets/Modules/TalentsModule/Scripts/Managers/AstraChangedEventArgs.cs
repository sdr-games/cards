using System;

using SDRGames.Whist.TalentsModule.Models;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class AstraChangedEventArgs : EventArgs
    { 
        public int TotalPoints { get; private set; }
        public Astra Astra { get; private set; }

        public AstraChangedEventArgs(Astra astra, int totalPoints)
        {
            Astra = astra;
            TotalPoints = totalPoints;
        }
    }
}