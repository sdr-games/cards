using System;
using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;

namespace SDRGames.Whist.AbilitiesQueueModule.Managers
{
    public class MeleeEndTurnEventArgs : EventArgs
    {
        public List<Ability> Abilities { get; private set; }

        public MeleeEndTurnEventArgs(List<Ability> abilities)
        {
            Abilities = abilities;
        }
    }
}