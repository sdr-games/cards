using System;

using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.ScriptableObjects
{
    public class AstraScriptableObject : TalentScriptableObject
    {
        [field: SerializeField][field: ReadOnly] public string Equipment { get; private set; }

        public void Initialize(string name, int cost, NodeTypes dialogueType, string equipment)
        {
            base.Initialize(name, cost, dialogueType);
            Equipment = equipment;
        }
    }
}
