using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    public class Group : UnityEditor.Experimental.GraphView.Group
    {
        public string ID { get; set; }
        public string OldTitle { get; set; }
        public List<BaseNodeSaveData> Nodes { get; set; }

        private Color defaultBorderColor;
        private float defaultBorderWidth;

        public Group(string groupTitle, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();

            title = groupTitle;
            OldTitle = groupTitle;

            Nodes = new List<BaseNodeSaveData>();

            SetPosition(new Rect(position, Vector2.zero));

            defaultBorderColor = contentContainer.style.borderBottomColor.value;
            defaultBorderWidth = contentContainer.style.borderBottomWidth.value;
        }

        public void SetErrorStyle(Color color)
        {
            contentContainer.style.borderBottomColor = color;
            contentContainer.style.borderBottomWidth = 2f;
        }

        public void ResetStyle()
        {
            contentContainer.style.borderBottomColor = defaultBorderColor;
            contentContainer.style.borderBottomWidth = defaultBorderWidth;
        }

        public void SaveToGraph(GraphSaveDataScriptableObject graphData)
        {
            GroupSaveData groupData = new GroupSaveData()
            {
                ID = ID,
                Name = title,
                Position = GetPosition().position,
            };

            graphData.Groups.Add(groupData);
        }

        public DialogueGroupScriptableObject SaveToSO(string folder)
        {
            string groupName = title;

            UtilityIO.CreateFolder($"{folder}/Groups", groupName);
            UtilityIO.CreateFolder($"{folder}/Groups/{groupName}", "Dialogues");

            DialogueGroupScriptableObject dialogueGroup = UtilityIO.CreateAsset<DialogueGroupScriptableObject>($"{folder}/Groups/{groupName}", groupName);

            dialogueGroup.Initialize(groupName);

            dialogueGroup.GroupedDialogues = new List<DialogueScriptableObject>();

            UtilityIO.SaveAsset(dialogueGroup);

            return dialogueGroup;
        }
    }
}