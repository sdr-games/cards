using System.Collections.Generic;

using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class ApplyButtonClickedEventArgs
    {
        public float TotalCost { get; private set; }
        public List<CardManager> Managers { get; private set; }

        public ApplyButtonClickedEventArgs(float totalCost, List<CardManager> managers)
        {
            TotalCost = totalCost;
            Managers = managers;
        }
    }
}