using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.ScriptableObjects
{
    public class AstraScriptableObject : TalentScriptableObject
    {
        [field: SerializeField][field: ReadOnly] public string Equipment { get; private set; }

        public void Initialize(string dialogueName, NodeTypes dialogueType, string equipment)
        {
            base.Initialize(dialogueName, dialogueType);
            Equipment = equipment;
        }
    }
}
