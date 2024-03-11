using System;

using UnityEngine;

using static SDRGames.Whist.DialogueSystem.Editor.Managers.GraphManager;

namespace SDRGames.Whist.DialogueSystem.Editor.Models
{
    [Serializable]
    public class BaseData
    {
        [field: SerializeField] public string ID { get; protected set; }
        [field: SerializeField] public string Name { get; protected set; }
        [field: SerializeField] public NodeTypes NodeType { get; protected set; }
        [field: SerializeField] public Vector2 Position { get; protected set; }

        public BaseData(string name, Vector2 position)
        {
            GenerateID();
            Name = name;
            Position = position;
        }

        public void GenerateID()
        {
            ID = Guid.NewGuid().ToString();
        }

        public virtual void SetName(string name)
        {
            Name = name;
        }

        public void SetNodeType(NodeTypes nodeType)
        {
            NodeType = nodeType;
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;
        } 
    }
}