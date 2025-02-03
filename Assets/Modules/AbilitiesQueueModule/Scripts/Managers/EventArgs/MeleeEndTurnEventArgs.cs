using System;
using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;

namespace SDRGames.Whist.AbilitiesQueueModule.Managers
{
    public class MeleeEndTurnEventArgs : EventArgs
    {
        public float TotalCost { get; private set; }
        public List<Ability> Abilities { get; private set; }

        public MeleeEndTurnEventArgs(float totalCost, List<Ability> abilities)
        {
            TotalCost = totalCost;
            Abilities = abilities;
        }
    }
}