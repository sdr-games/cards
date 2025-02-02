using System;

namespace SDRGames.Whist.AbilitiesQueueModule.Managers
{
    public class AbilityQueueCountChangedEventArgs : EventArgs
    {
        public bool IsEmpty { get; private set; }

        public AbilityQueueCountChangedEventArgs(bool isEmpty)
        {
            IsEmpty = isEmpty;
        }
    }
}