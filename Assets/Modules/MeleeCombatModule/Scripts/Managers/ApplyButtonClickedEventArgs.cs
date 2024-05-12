using System;
using System.Collections.Generic;

using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;

namespace SDRGames.Whist.MeleeCombatModule.Managers
{
    public class ApplyButtonClickedEventArgs : EventArgs
    {
        public List<MeleeAttackScriptableObject> MeleeAttackScriptableObjects { get; private set; }

        public ApplyButtonClickedEventArgs(List<MeleeAttackScriptableObject> meleeAttackScriptableObject)
        {
            MeleeAttackScriptableObjects = meleeAttackScriptableObject;
        }
    }
}