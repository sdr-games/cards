using System.Collections.Generic;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.ScriptableObjects
{
    public class DialogueGroupScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string GroupName { get; set; }
        [field: SerializeField] public List<DialogueScriptableObject> GroupedDialogues { get; set; }

        public void Initialize(string groupName)
        {
            GroupName = groupName;
            GroupedDialogues = new List<DialogueScriptableObject>();
        }
    }
}