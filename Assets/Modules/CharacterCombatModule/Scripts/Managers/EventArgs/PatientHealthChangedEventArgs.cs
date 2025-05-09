using System;

namespace SDRGames.Whist.CharacterCombatModule.Managers
{
    public class PatientHealthChangedEventArgs : EventArgs
    {
        public int CurrentHealth { get; private set; }

        public PatientHealthChangedEventArgs(int currentHealth)
        {
            CurrentHealth = currentHealth;
        }
    }
}