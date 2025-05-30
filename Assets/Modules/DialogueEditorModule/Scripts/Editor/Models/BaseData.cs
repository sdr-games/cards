using System;

using SDRGames.Whist.DialogueModule.ScriptableObjects;

using UnityEditor;

using UnityEngine;

using static SDRGames.Whist.DialogueModule.ScriptableObjects.DialogueScriptableObject;

namespace SDRGames.Whist.DialogueEditorModule.Models
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

        public virtual DialogueScriptableObject SaveToSO(DialogueScriptableObject dialogueSO)
        {
            dialogueSO.Initialize(
                NodeName,
                NodeType
            );
            UtilityIO.SaveAsset(dialogueSO);
            EditorUtility.SetDirty(dialogueSO);
            return dialogueSO;
        }
    }
}