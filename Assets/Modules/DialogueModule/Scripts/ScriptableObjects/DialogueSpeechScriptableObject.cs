using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Models;

using UnityEngine;

using static SDRGames.Whist.DialogueSystem.Editor.Managers.GraphManager;

namespace SDRGames.Whist.DialogueSystem.ScriptableObjects
{
    public class DialogueSpeechScriptableObject : DialogueScriptableObject
    {
        [field: SerializeField] public DialogueLocalizationData CharacterNameLocalization { get; private set; }
        [field: SerializeField] public DialogueLocalizationData TextLocalization { get; set; }

        public void Initialize(string dialogueName, DialogueLocalizationData characterNameLocalization, DialogueLocalizationData textLocalization, List<DialogueAnswerData> answers, NodeTypes dialogueType)
        {
            base.Initialize(dialogueName, answers, dialogueType);
            CharacterNameLocalization = characterNameLocalization;
            TextLocalization = textLocalization;
        }
    }
}
