using System;

namespace SDRGames.Whist.CharacterCombatModule.ScriptableObjects
{
    public class MagicalDamageChangedEventArgs : EventArgs
    {
        public int MagicDamage { get; private set; }

        public MagicalDamageChangedEventArgs(int magicDamageMultiplier)
        {
            MagicDamage = magicDamageMultiplier;
        }
    }
}