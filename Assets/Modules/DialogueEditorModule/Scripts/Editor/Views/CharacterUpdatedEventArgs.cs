﻿using System;

using SDRGames.Whist.CharacterInfoModule.ScriptableObjects;

namespace SDRGames.Whist.DialogueEditorModule.Views
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