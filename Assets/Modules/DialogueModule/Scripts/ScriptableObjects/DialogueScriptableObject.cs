using UnityEngine;

using static SDRGames.Whist.DialogueSystem.Editor.Managers.GraphManager;

namespace SDRGames.Whist.DialogueSystem.ScriptableObjects
{
    public class DialogueScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public NodeTypes DialogueType { get; private set; }

        public virtual void Initialize(string dialogueName, NodeTypes dialogueType)
        {
            Name = dialogueName;
            DialogueType = dialogueType;
        }
    }
}