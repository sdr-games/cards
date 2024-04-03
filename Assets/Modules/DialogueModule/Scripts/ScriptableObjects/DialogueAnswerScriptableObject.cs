using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.LocalizationModule.Models;

using UnityEngine;

namespace SDRGames.Whist.DialogueModule.ScriptableObjects
{
    public class DialogueAnswerScriptableObject : DialogueScriptableObject
    {
        [field: SerializeField] public CharacterInfoScriptableObject Character { get; private set; }
        [field: SerializeField] public LocalizationData TextLocalization { get; private set; }
        [field: SerializeField] public DialogueSpeechScriptableObject NextSpeech { get; private set; }

        public void Initialize(string dialogueName, NodeTypes dialogueType, CharacterInfoScriptableObject character, LocalizationData textLocalization)
        {
            base.Initialize(dialogueName, dialogueType);
            Character = character;
            TextLocalization = textLocalization;
        }

        public void SetNextSpech(DialogueSpeechScriptableObject nextSpeech)
        {
            NextSpeech = nextSpeech;
        }

        public override int GetCharactersCount()
        {
            return TextLocalization.GetLocalizedText().Length;
        }
    }
}
