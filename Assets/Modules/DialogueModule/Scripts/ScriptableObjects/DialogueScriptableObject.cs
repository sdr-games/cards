using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Models;

using UnityEngine;

using static SDRGames.Whist.DialogueSystem.Editor.Managers.GraphManager;

namespace SDRGames.Whist.DialogueSystem.ScriptableObjects
{
    public class DialogueScriptableObject : ScriptableObject
    {
        //public enum DialogueQuestActions { No, Accept, Decline, Finish };

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public List<DialogueAnswerData> Answers { get; private set; }
        [field: SerializeField] public NodeTypes DialogueType { get; private set; }

        public virtual void Initialize(string dialogueName, List<DialogueAnswerData> answers, NodeTypes dialogueType)
        {
            Name = dialogueName;
            Answers = answers;
            DialogueType = dialogueType;
        }
    }
}