using System;

namespace SDRGames.Whist.CharacterModule.Models
{
    public class PeriodicalEffect
    {
        public int Duration { get; private set; }
        public Action<int> Action { get; private set; }

        public event EventHandler<DurationChangedEventArgs> DurationChanged;

        public PeriodicalEffect(int duration, Action<int> action)
        {
            Duration = duration;
            Action = action;
        }

        public void IncreaseDuration(int additionalDuration)
        {
            Duration += additionalDuration;
            DurationChanged?.Invoke(this, new DurationChangedEventArgs(Duration));
        }

        public void DecreaseDuration(int duration)
        {
            Duration -= duration;
            DurationChanged?.Invoke(this, new DurationChangedEventArgs(Duration));
        }
    }
}
