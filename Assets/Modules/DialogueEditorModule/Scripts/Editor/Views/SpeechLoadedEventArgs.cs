using System;

using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.LocalizationModule.Models;
using UnityEngine;

namespace SDRGames.Whist.DialogueEditorModule.Views
{
    public class SpeechLoadedEventArgs : EventArgs
    {
        [field: SerializeField] public CharacterInfoScriptableObject Character { get; private set; }
        [field: SerializeField] public LocalizationData TextLocalization { get; private set; }

        public SpeechLoadedEventArgs(CharacterInfoScriptableObject character, LocalizationData textLocalization)
        {
            Character = character;
            TextLocalization = textLocalization;
        }
    }
}