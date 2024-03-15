using System;

using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Managers
{
    public class DialogueManager : MonoBehaviour
    {
        [field: SerializeField] public DialogueContainerScriptableObject DialogueContainer { get; private set; }

        private void Start()
        {
            Initialize(DialogueContainer);
        }

        public void Initialize(DialogueContainerScriptableObject dialogueContainer)
        {
            DialogueContainer = dialogueContainer;
        }
    }
}