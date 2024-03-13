using SDRGames.Whist.DialogueSystem.Models;
using UnityEngine;

using static SDRGames.Whist.DialogueSystem.Editor.Managers.GraphManager;

namespace SDRGames.Whist.DialogueSystem.ScriptableObjects
{
    public class DialogueAnswerScriptableObject : DialogueScriptableObject
    {
        [field: SerializeField] public LocalizationData CharacterNameLocalization { get; private set; }
        [field: SerializeField] public LocalizationData TextLocalization { get; private set; }
        [field: SerializeField] public DialogueSpeechScriptableObject NextSpeech { get; private set; }

        public void Initialize(string dialogueName, NodeTypes dialogueType, LocalizationData characterNameLocalization, LocalizationData textLocalization)
        {
            base.Initialize(dialogueName, dialogueType);
            CharacterNameLocalization = characterNameLocalization;
            TextLocalization = textLocalization;
        }

        public void SetNextSpech(DialogueSpeechScriptableObject nextSpeech)
        {
            NextSpeech = nextSpeech;
        }
    }
}
