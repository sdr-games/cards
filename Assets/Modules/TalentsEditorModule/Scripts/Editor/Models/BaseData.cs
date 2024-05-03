using System;

using SDRGames.Whist.LocalizationModule.Models;

using UnityEngine;

using static SDRGames.Whist.TalentsModule.ScriptableObjects.TalentScriptableObject;

namespace SDRGames.Whist.TalentsEditorModule.Models
{
    [Serializable]
    public class BaseData
    {
        [field: SerializeField] public string ID { get; protected set; }
        [field: SerializeField] public string NodeName { get; protected set; }
        [field: SerializeField] public LocalizationData DescriptionLocalization { get; protected set; }
        [field: SerializeField] public int Cost { get; protected set; }
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

        public virtual void SetCost(int cost)
        {
            Cost = cost;
        }

        public virtual void SetDescription(LocalizationData descriptionLocalization)
        {
            DescriptionLocalization = descriptionLocalization;
        }

        public void SetNodeType(NodeTypes nodeType)
        {
            NodeType = nodeType;
        }
    }
}