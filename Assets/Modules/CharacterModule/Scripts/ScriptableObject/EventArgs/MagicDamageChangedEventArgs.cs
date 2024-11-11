using System;

namespace SDRGames.Whist.CharacterModule.ScriptableObjects
{
    public class MagicDamageChangedEventArgs : EventArgs
    {
        public int MagicDamage { get; private set; }

        public MagicDamageChangedEventArgs(int magicDamageMultiplier)
        {
            MagicDamage = magicDamageMultiplier;
        }
    }
}