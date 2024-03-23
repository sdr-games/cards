using SDRGames.Whist.DialogueModule.Editor;

using UnityEngine;

using static SDRGames.Whist.DialogueModule.Editor.Managers.GraphManager;

namespace SDRGames.Whist.DialogueModule.ScriptableObjects
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

        public virtual int GetCharactersCount()
        {
            return 0;
        }
    }
}