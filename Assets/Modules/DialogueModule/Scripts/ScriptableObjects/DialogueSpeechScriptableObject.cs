using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Models;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.ScriptableObjects
{
    public class DialogueSpeechScriptableObject : DialogueScriptableObject
    {
        public enum DialogueActions { No, Trade, Craft };

        [field: SerializeField] public DialogueLocalizationData LocalizationData { get; set; }
        [field: SerializeField] public Quest Quest { get; set; }
        [field: SerializeField] public DialogueQuestActions DialogueQuestAction { get; set; }
        [field: SerializeField] public DialogueActions DialogueAction { get; set; }

        public void Initialize(string dialogueName, DialogueLocalizationData localizationData, List<DialogueAnswerData> answers, DialogueTypes dialogueType, Quest quest, DialogueQuestActions dialogueQuestAction, DialogueActions dialogueAction)
        {
            base.Initialize(dialogueName, answers, dialogueType);
            LocalizationData = localizationData;
            Quest = quest;
            DialogueQuestAction = dialogueQuestAction;
            DialogueAction = dialogueAction;
        }
    }
}
