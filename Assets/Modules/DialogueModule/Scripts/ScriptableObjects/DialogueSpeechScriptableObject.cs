using System.Collections.Generic;

using SDRGames.Whist.CharacterInfoModule.ScriptableObjects;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.LocalizationModule.Models;

using UnityEngine;

namespace SDRGames.Whist.DialogueModule.ScriptableObjects
{
    public class DialogueSpeechScriptableObject : DialogueScriptableObject
    {
        [field: SerializeField][field: ReadOnly] public CharacterInfoScriptableObject Character { get; private set; }
        [field: SerializeField][field: ReadOnly] public LocalizationData TextLocalization { get; private set; }
        [field: SerializeField][field: ReadOnly] public List<DialogueAnswerScriptableObject> Answers { get; private set; }

        public void Initialize(string dialogueName, NodeTypes dialogueType, CharacterInfoScriptableObject character, LocalizationData textLocalization)
        {
            base.Initialize(dialogueName, dialogueType);
            Character = character;
            TextLocalization = textLocalization;
            Answers = new List<DialogueAnswerScriptableObject>();
        }

        public override int GetCharactersCount()
        {
            return TextLocalization.GetLocalizedText().Length;
        }
    }
}
