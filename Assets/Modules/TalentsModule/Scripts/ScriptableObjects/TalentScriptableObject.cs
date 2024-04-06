using System.Collections.Generic;

using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.ScriptableObjects
{
    public class TalentScriptableObject : ScriptableObject
    {
        public enum NodeTypes { Talamus = 0, Astra = 1 };
        [field: SerializeField][field: ReadOnly] public string Name { get; private set; }
        [field: SerializeField][field: ReadOnly] public NodeTypes TalentType { get; private set; }

        public virtual void Initialize(string dialogueName, NodeTypes talentType)
        {
            Name = dialogueName;
            TalentType = talentType;
        }
    }
}
