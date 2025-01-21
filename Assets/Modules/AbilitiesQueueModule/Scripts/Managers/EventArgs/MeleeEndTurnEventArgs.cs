using System;
using System.Collections.Generic;

using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;

namespace SDRGames.Whist.AbilitiesQueueModule.Managers
{
    public class MeleeEndTurnEventArgs : EventArgs
    {
        public float TotalCost { get; private set; }
        public List<AbilityScriptableObject> Abilities { get; private set; }

        public MeleeEndTurnEventArgs(float totalCost, List<AbilityScriptableObject> abilities)
        {
            TotalCost = totalCost;
            Abilities = abilities;
        }
    }
}