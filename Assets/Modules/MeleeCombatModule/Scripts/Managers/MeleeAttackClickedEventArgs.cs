using System;

using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;

namespace SDRGames.Whist.MeleeCombatModule.Managers
{
    public class MeleeAttackClickedEventArgs : EventArgs
    {
        public MeleeAttackScriptableObject MeleeAttackScriptableObject { get; private set; }

        public MeleeAttackClickedEventArgs(MeleeAttackScriptableObject meleeAttackScriptableObject)
        {
            MeleeAttackScriptableObject = meleeAttackScriptableObject;
        }
    }
}