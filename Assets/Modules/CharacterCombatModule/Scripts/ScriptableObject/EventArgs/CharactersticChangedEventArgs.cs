using System;

namespace SDRGames.Whist.CharacterCombatModule.ScriptableObjects
{
    public class CharactersticChangedEventArgs : EventArgs
    {
        public int CharactersticValue { get; private set; }

        public CharactersticChangedEventArgs(int charactersticValue)
        {
            CharactersticValue = charactersticValue;
        }
    }
}