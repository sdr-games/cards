using System;

namespace SDRGames.Whist.TalentsModule.Models
{
    public class CurrentPointsChangedEventArgs : EventArgs
    {
        public int CurrentPoints { get; private set; }

        public CurrentPointsChangedEventArgs(int currentPoints)
        {
            CurrentPoints = currentPoints;
        }
    }
}