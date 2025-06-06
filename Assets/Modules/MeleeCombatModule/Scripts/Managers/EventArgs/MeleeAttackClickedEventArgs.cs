using System;

using SDRGames.Whist.MeleeCombatModule.Models;

namespace SDRGames.Whist.MeleeCombatModule.Managers
{
    public class MeleeAttackClickedEventArgs : EventArgs
    {
        public MeleeAttack MeleeAttack { get; private set; }

        public MeleeAttackClickedEventArgs(MeleeAttack meleeAttack)
        {
            MeleeAttack = meleeAttack;
        }
    }
}