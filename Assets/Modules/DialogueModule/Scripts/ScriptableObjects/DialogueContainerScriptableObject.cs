using System.Collections.Generic;

using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.DialogueModule.ScriptableObjects
{
    public class DialogueContainerScriptableObject : ScriptableObject
    {
        [field: SerializeField][field: ReadOnly] public string FileName { get; set; }
        [field: SerializeField][field: ReadOnly] public List<DialogueScriptableObject> Dialogues { get; set; }
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
            DialogueStartScriptableObject start = Dialogues.Find(item => item.GetType() == typeof(DialogueStartScriptableObject)) as DialogueStartScriptableObject;
            return start.NextSpeech;
        }
    }
}