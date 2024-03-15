using System.Collections.Generic;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.ScriptableObjects
{
    public class DialogueContainerScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<DialogueScriptableObject> Dialogues { get; set; }
        public DialogueSpeechScriptableObject FirstSpeech => GetFirstSpeech();

        public void Initialize(string fileName)
        {
            FileName = fileName;

            Dialogues = new List<DialogueScriptableObject>();
        }

        public List<string> GetDialogueNames()
        {
            List<string> dialogueNames = new List<string>();

            foreach (DialogueScriptableObject dialogue in Dialogues)
            {
                dialogueNames.Add(dialogue.Name);
            }

            return dialogueNames;
        }

        private DialogueSpeechScriptableObject GetFirstSpeech()
        {
            //DialogueScriptableObject start = Dialogues.Find(item => item.GetType() == typeof(DialogueScriptableObject));
            //return start.Answers[0].NextDialogue as DialogueSpeechScriptableObject;
            return null;
        }
    }
}