using System;

namespace SDRGames.Whist.CharacterModule.Models
{
    public class DurationChangedEventArgs : EventArgs
    {
        public int Duration { get; private set; }

        public DurationChangedEventArgs(int duration)
        {
            Duration = duration;
        }
    }
}