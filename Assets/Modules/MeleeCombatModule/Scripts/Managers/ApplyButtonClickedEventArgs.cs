using System;
using System.Collections.Generic;

using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;

namespace SDRGames.Whist.MeleeCombatModule.Managers
{
    public class ApplyButtonClickedEventArgs : EventArgs
    {
        public float TotalCost { get; private set; }
        public List<MeleeAttackScriptableObject> MeleeAttackScriptableObjects { get; private set; }

        public ApplyButtonClickedEventArgs(float totalCost, List<MeleeAttackScriptableObject> meleeAttackScriptableObject)
        {
            TotalCost = totalCost;
            MeleeAttackScriptableObjects = meleeAttackScriptableObject;
        }
    }
}