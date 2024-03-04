using System;

using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Views
{
    [Serializable]
    public class DDialogue
    {
        /* Dialogue Scriptable Objects */
        [SerializeField] public DialogueContainerScriptableObject dialogueContainer;
        [SerializeField] public DialogueGroupScriptableObject dialogueGroup;
        [SerializeField] public DialogueScriptableObject dialogue;

        /* Filters */
        [SerializeField] private bool groupedDialogues;
        [SerializeField] private bool startingDialoguesOnly;

        /* Indexes */
        [SerializeField] private int selectedDialogueGroupIndex;
        [SerializeField] private int selectedDialogueIndex;
    }
}