using System;

namespace SDRGames.Whist.CharacterCombatModule.Models
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