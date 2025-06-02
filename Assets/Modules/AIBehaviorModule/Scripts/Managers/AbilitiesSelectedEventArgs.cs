using System;

namespace SDRGames.Whist.EnemyBehaviorModule.Managers
{
    public class AbilitiesSelectedEventArgs : EventArgs
    {
        public bool ActiveBlockPossible { get; private set; }

        public AbilitiesSelectedEventArgs(bool activeBlockPossible)
        {
            ActiveBlockPossible = activeBlockPossible;
        }
    }
}