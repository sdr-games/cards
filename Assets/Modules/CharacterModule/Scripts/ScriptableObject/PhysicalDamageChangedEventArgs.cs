using System;

namespace SDRGames.Whist.CharacterModule.ScriptableObjects
{
    public class PhysicalDamageChangedEventArgs : EventArgs
    {
        public int PhysicalDamage { get; private set; }

        public PhysicalDamageChangedEventArgs(int physicalDamage)
        {
            PhysicalDamage = physicalDamage;
        }
    }
}