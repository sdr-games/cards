using System;

using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEngine;

using static SDRGames.Whist.DialogueSystem.Editor.Managers.GraphManager;

namespace SDRGames.Whist.DialogueSystem.Editor.Models
{
    [Serializable]
    public class BaseData
    {
        [field: SerializeField] public string ID { get; protected set; }
        [field: SerializeField] public string NodeName { get; protected set; }
        [field: SerializeField] public NodeTypes NodeType { get; protected set; }

        public BaseData(string nodeName, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();
            NodeName = nodeName;
        }

        public virtual void SetNodeName(string nodeName)
        {
            NodeName = nodeName;
        }

        public void SetNodeType(NodeTypes nodeType)
        {
            NodeType = nodeType;
        }

        public virtual DialogueScriptableObject SaveToSO(DialogueScriptableObject dialogueSO)
        {
            dialogueSO.Initialize(
                NodeName,
                NodeType
            );
            return dialogueSO;
        }
    }
}