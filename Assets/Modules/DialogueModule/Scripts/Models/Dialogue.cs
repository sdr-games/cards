using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Models
{
    public class Dialogue
    {
        [field: SerializeField] public DialogueContainerScriptableObject DialogueContainerScriptableObject { get; private set; }
    }
}
