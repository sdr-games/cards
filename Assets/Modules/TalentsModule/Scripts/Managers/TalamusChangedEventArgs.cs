using System;

using SDRGames.Whist.TalentsModule.Models;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class TalamusChangedEventArgs : EventArgs
    {
        public int TotalPoints { get; private set; }
        public Talamus Talamus { get; private set; }

        public TalamusChangedEventArgs(Talamus talamus, int totalPoints)
        {
            Talamus = talamus;
            TotalPoints = totalPoints;
        }
    }
}