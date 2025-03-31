using System;

namespace SDRGames.Whist.CharacterModule.Managers
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