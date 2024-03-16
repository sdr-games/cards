using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Managers
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private DialogueContainerScriptableObject _dialogueContainer;

        private void Start()
        {
            Initialize(_dialogueContainer);
        }

        public void Initialize(DialogueContainerScriptableObject dialogueContainer)
        {
            _dialogueContainer = dialogueContainer;
        }
    }
}