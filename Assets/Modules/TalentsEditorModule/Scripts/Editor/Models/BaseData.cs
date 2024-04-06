using System;

using SDRGames.Whist.TalentsModule.ScriptableObjects;

using UnityEngine;

using static SDRGames.Whist.TalentsModule.ScriptableObjects.TalentScriptableObject;

namespace SDRGames.Whist.TalentsEditorModule.Models
{
    [Serializable]
    public class BaseData
    {
        [field: SerializeField] public string ID { get; protected set; }
        [field: SerializeField] public string NodeName { get; protected set; }
        [field: SerializeField] public NodeTypes NodeType { get; protected set; }

        public BaseData(string nodeName)
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

        public virtual TalentScriptableObject SaveToSO(TalentScriptableObject dialogueSO)
        {
            dialogueSO.Initialize(
                NodeName,
                NodeType
            );
            return dialogueSO;
        }
    }
}