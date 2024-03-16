using SDRGames.Whist.DialogueSystem.Models;
using UnityEngine;

using static SDRGames.Whist.DialogueSystem.Editor.Managers.GraphManager;

namespace SDRGames.Whist.DialogueSystem.ScriptableObjects
{
    public class DialogueAnswerScriptableObject : DialogueScriptableObject
    {
        [field: SerializeField] public DialogueCharacterScriptableObject Character { get; private set; }
        [field: SerializeField] public LocalizationData TextLocalization { get; private set; }
        [field: SerializeField] public DialogueSpeechScriptableObject NextSpeech { get; private set; }

        public void Initialize(string dialogueName, NodeTypes dialogueType, DialogueCharacterScriptableObject character, LocalizationData textLocalization)
        {
            base.Initialize(dialogueName, dialogueType);
            Character = character;
            TextLocalization = textLocalization;
        }

        public void SetNextSpech(DialogueSpeechScriptableObject nextSpeech)
        {
            NextSpeech = nextSpeech;
        }
    }
}
