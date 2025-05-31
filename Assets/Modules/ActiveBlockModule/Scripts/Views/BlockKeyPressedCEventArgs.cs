using System;

namespace SDRGames.Whist.ActiveBlockModule.Views
{
    public class BlockKeyPressedCEventArgs : EventArgs
    {
        public float DamageMultiplier { get; private set; }

        public BlockKeyPressedCEventArgs(float damageMultiplier)
        {
            DamageMultiplier = damageMultiplier;
        }
    }
}