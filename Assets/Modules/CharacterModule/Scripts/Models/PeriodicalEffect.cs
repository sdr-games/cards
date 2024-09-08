using System;

namespace SDRGames.Whist.CharacterModule.Models
{
    public class PeriodicalEffect
    {
        public int Duration { get; private set; }
        public Action Action { get; private set; }

        public PeriodicalEffect(int duration, Action action)
        {
            Duration = duration;
            Action = action;
        }

        public void IncreaseDuration(int additionalDuration)
        {
            Duration += additionalDuration;
        }
    }
}
