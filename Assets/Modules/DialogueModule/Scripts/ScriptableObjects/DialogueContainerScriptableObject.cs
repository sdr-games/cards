using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Helpers;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.ScriptableObjects
{
    public class DialogueContainerScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public SerializableDictionary<DialogueGroupScriptableObject, List<DialogueScriptableObject>> DialogueGroups { get; set; }
        [field: SerializeField] public List<DialogueScriptableObject> UngroupedDialogues { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;

            DialogueGroups = new SerializableDictionary<DialogueGroupScriptableObject, List<DialogueScriptableObject>>();
            UngroupedDialogues = new List<DialogueScriptableObject>();
        }

        public List<string> GetDialogueGroupNames()
        {
            List<string> dialogueGroupNames = new List<string>();

            foreach (DialogueGroupScriptableObject dialogueGroup in DialogueGroups.Keys)
            {
                dialogueGroupNames.Add(dialogueGroup.GroupName);
            }

            return dialogueGroupNames;
        }

        public List<string> GetGroupedDialogueNames(DialogueGroupScriptableObject dialogueGroup, bool startingDialoguesOnly)
        {
            List<DialogueScriptableObject> groupedDialogues = DialogueGroups[dialogueGroup];

            List<string> groupedDialogueNames = new List<string>();

            foreach (DialogueScriptableObject groupedDialogue in groupedDialogues)
            {
                groupedDialogueNames.Add(groupedDialogue.DialogueName);
            }

            return groupedDialogueNames;
        }

        public List<string> GetUngroupedDialogueNames(bool startingDialoguesOnly)
        {
            List<string> ungroupedDialogueNames = new List<string>();

            foreach (DialogueScriptableObject ungroupedDialogue in UngroupedDialogues)
            {
                ungroupedDialogueNames.Add(ungroupedDialogue.DialogueName);
            }

            return ungroupedDialogueNames;
        }

        public List<DialogueScriptableObject> GetGroupedDialogues(DialogueGroupScriptableObject dialogueGroup)
        {
            return DialogueGroups[dialogueGroup];
        }
    }
}