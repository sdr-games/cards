using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.DialogueModule.ScriptableObjects
{
    public class DialogueScriptableObject : ScriptableObject
    {
        public enum NodeTypes { Start = 0, Speech = 1, Answer = 2 };
        [field: SerializeField][field: ReadOnly] public string Name { get; private set; }
        [field: SerializeField][field: ReadOnly] public NodeTypes DialogueType { get; private set; }

        public virtual void Initialize(string dialogueName, NodeTypes dialogueType)
        {
            Name = dialogueName;
            DialogueType = dialogueType;
        }

        public virtual int GetCharactersCount()
        {
            return 0;
        }
    }
}