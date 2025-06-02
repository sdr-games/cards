using System;

namespace SDRGames.Whist.ActiveBlockModule.Views
{
    public class BlockKeyPressedEventArgs : EventArgs
    {
        public float DamageMultiplier { get; private set; }

        public BlockKeyPressedEventArgs(float damageMultiplier = 1)
        {
            DamageMultiplier = damageMultiplier;
        }
    }
}