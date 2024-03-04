using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Models;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.ScriptableObjects
{
    public class DialogueScriptableObject : ScriptableObject
    {
        public enum DialogueQuestActions { No, Accept, Decline, Finish };
        public enum DialogueTypes { Start = 0, Speech = 1 };

        [field: SerializeField] public string DialogueName { get; set; }
        [field: SerializeField] public List<DialogueAnswerData> Answers { get; set; }
        [field: SerializeField] public DialogueTypes DialogueType { get; set; }

        public virtual void Initialize(string dialogueName, List<DialogueAnswerData> answers, DialogueTypes dialogueType)
        {
            DialogueName = dialogueName;
            Answers = answers;
            DialogueType = dialogueType;
        }
    }
}