using System;

using SDRGames.Whist.DialogueSystem.ScriptableObjects;

namespace SDRGames.Whist.DialogueSystem.Editor.Views
{
    public class CharacterUpdatedEventArgs : EventArgs
    {
        public DialogueCharacterScriptableObject Character { get; private set; }

        public CharacterUpdatedEventArgs(DialogueCharacterScriptableObject character)
        {
            Character = character;
        }
    }
}