using System;

using SDRGames.Whist.CharacterModule.ScriptableObjects;

namespace SDRGames.Whist.DialogueModule.Editor.Views
{
    public class CharacterUpdatedEventArgs : EventArgs
    {
        public CharacterInfoScriptableObject Character { get; private set; }

        public CharacterUpdatedEventArgs(CharacterInfoScriptableObject character)
        {
            Character = character;
        }
    }
}