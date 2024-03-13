using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Models;

using UnityEngine;

using static SDRGames.Whist.DialogueSystem.Editor.Managers.GraphManager;

namespace SDRGames.Whist.DialogueSystem.ScriptableObjects
{
    public class DialogueSpeechScriptableObject : DialogueScriptableObject
    {
        [field: SerializeField] public LocalizationData CharacterNameLocalization { get; private set; }
        [field: SerializeField] public LocalizationData TextLocalization { get; private set; }
        [field: SerializeField] public List<DialogueAnswerScriptableObject> Answers { get; private set; }

        public void Initialize(string dialogueName, NodeTypes dialogueType, LocalizationData characterNameLocalization, LocalizationData textLocalization)
        {
            base.Initialize(dialogueName, dialogueType);
            CharacterNameLocalization = characterNameLocalization;
            TextLocalization = textLocalization;
            Answers = new List<DialogueAnswerScriptableObject>();
        }
    }
}
