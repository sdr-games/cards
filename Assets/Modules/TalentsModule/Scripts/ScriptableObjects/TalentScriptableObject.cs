using System;
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
        [field: SerializeField][field: ReadOnly] public Vector2 Position { get; private set; }
        [field: SerializeField][field: ReadOnly] public List<TalentScriptableObject> Blockers { get; private set; }
        [field: SerializeField][field: ReadOnly] public List<TalentScriptableObject> Dependencies { get; private set; }

        public virtual void Initialize(string name, NodeTypes talentType)
        {
            Name = name;
            TalentType = talentType;
            Blockers = new List<TalentScriptableObject>();
            Dependencies = new List<TalentScriptableObject>();
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;
        }
    }
}
