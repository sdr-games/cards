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
        [SerializeField] public DialogueScriptableObject dialogue;

        /* Indexes */
        [SerializeField] private int selectedDialogueIndex;
    }
}