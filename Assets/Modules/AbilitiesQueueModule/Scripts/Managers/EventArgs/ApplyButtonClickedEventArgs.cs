using System;
using System.Collections.Generic;

using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;

namespace SDRGames.Whist.AbilitiesQueueModule.Managers
{
    public class ApplyButtonClickedEventArgs : EventArgs
    {
        public float TotalCost { get; private set; }
        public List<AbilityScriptableObject> AbilityScriptableObjects { get; private set; }

        public ApplyButtonClickedEventArgs(float totalCost, List<AbilityScriptableObject> abilityScriptableObject)
        {
            TotalCost = totalCost;
            AbilityScriptableObjects = abilityScriptableObject;
        }
    }
}