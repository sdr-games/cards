using System;
using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    [CustomEditor(typeof(TriggerBehaviorDialogueController))]
    public class TriggerBehaviorDialogueEditorInspector : UnityEditor.Editor
    {
        private SerializedProperty dialogueContainerProperty;
        private SerializedProperty dialogueGroupProperty;
        private SerializedProperty dialogueProperty;

        private SerializedProperty selectedDialogueGroupIndexProperty;
        private SerializedProperty selectedDialogueIndexProperty;

        private SerializedProperty repeatableProperty;

        private void OnEnable()
        {
            dialogueContainerProperty = serializedObject.FindProperty("_dialogueContainer");
            dialogueGroupProperty = serializedObject.FindProperty("dialogueGroup");
            dialogueProperty = serializedObject.FindProperty("dialogue");

            selectedDialogueGroupIndexProperty = serializedObject.FindProperty("_selectedDialogueGroupIndex");
            selectedDialogueIndexProperty = serializedObject.FindProperty("_selectedDialogueIndex");

            repeatableProperty = serializedObject.FindProperty("repeatable");
        }

        public override void OnInspectorGUI()
        {
            List<string> dialogueNames;
            string dialogueInfoMessage;

            serializedObject.Update();
            DialogueContainerScriptableObject currentDialogueContainer = Resources.Load("ScriptableObjectsAssets/Dialogues/Triggers/Triggers", typeof(DialogueContainerScriptableObject)) as DialogueContainerScriptableObject;
            dialogueContainerProperty.objectReferenceValue = currentDialogueContainer;
            DrawDialogueContainerArea();

            string dialogueFolderPath = $"Assets/Resources/ScriptableObjectsAssets/Dialogues/{currentDialogueContainer.FileName}";
            List<string> dialogueGroupNames = currentDialogueContainer.GetDialogueGroupNames();

            if (dialogueGroupNames.Count == 0)
            {
                StopDrawing("There are no Dialogue Groups in this Dialogue Container.");
                return;
            }

            DrawDialogueGroupArea(currentDialogueContainer, dialogueGroupNames);

            DialogueGroupScriptableObject dialogueGroup = (DialogueGroupScriptableObject)dialogueGroupProperty.objectReferenceValue;
            dialogueNames = currentDialogueContainer.GetGroupedDialogueNames(dialogueGroup, false);
            dialogueFolderPath += $"/Groups/{dialogueGroup.GroupName}/Dialogues";
            dialogueInfoMessage = "There are no Dialogues in this Dialogue Group.";

            if (dialogueNames.Count == 0)
            {
                StopDrawing(dialogueInfoMessage);
                return;
            }

            DrawDialogueArea(dialogueNames, dialogueFolderPath);

            DrawTriggerParamsArea();

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawDialogueContainerArea()
        {
            UtilityInspector.DrawDisabledFields(() => dialogueContainerProperty.DrawPropertyField());
        }

        private void DrawDialogueGroupArea(DialogueContainerScriptableObject dialogueContainer, List<string> dialogueGroupNames)
        {
            int oldSelectedDialogueGroupIndex = selectedDialogueGroupIndexProperty.intValue;

            DialogueGroupScriptableObject oldDialogueGroup = (DialogueGroupScriptableObject)dialogueGroupProperty.objectReferenceValue;

            bool isOldDialogueGroupNull = oldDialogueGroup == null;

            string oldDialogueGroupName = isOldDialogueGroupNull ? "" : oldDialogueGroup.GroupName;

            UpdateIndexOnNamesListUpdate(dialogueGroupNames, selectedDialogueGroupIndexProperty, oldSelectedDialogueGroupIndex, oldDialogueGroupName, isOldDialogueGroupNull);

            selectedDialogueGroupIndexProperty.intValue = UtilityInspector.DrawPopup("Dialogue Group", selectedDialogueGroupIndexProperty, dialogueGroupNames.ToArray());

            string selectedDialogueGroupName = dialogueGroupNames[selectedDialogueGroupIndexProperty.intValue];

            DialogueGroupScriptableObject selectedDialogueGroup = UtilityIO.LoadAsset<DialogueGroupScriptableObject>($"Assets/Resources/ScriptableObjectsAssets/Dialogues/{dialogueContainer.FileName}/Groups/{selectedDialogueGroupName}", selectedDialogueGroupName);

            dialogueGroupProperty.objectReferenceValue = selectedDialogueGroup;
        }

        private void DrawDialogueArea(List<string> dialogueNames, string dialogueFolderPath)
        {
            int oldSelectedDialogueIndex = selectedDialogueIndexProperty.intValue;

            DialogueScriptableObject oldDialogue = (DialogueScriptableObject)dialogueProperty.objectReferenceValue;

            bool isOldDialogueNull = oldDialogue == null;

            string oldDialogueName = isOldDialogueNull ? "" : oldDialogue.DialogueName;

            UpdateIndexOnNamesListUpdate(dialogueNames, selectedDialogueIndexProperty, oldSelectedDialogueIndex, oldDialogueName, isOldDialogueNull);

            selectedDialogueIndexProperty.intValue = UtilityInspector.DrawPopup("Dialogue", selectedDialogueIndexProperty, dialogueNames.ToArray());

            string selectedDialogueName = dialogueNames[selectedDialogueIndexProperty.intValue];

            DialogueScriptableObject selectedDialogue = UtilityIO.LoadAsset<DialogueScriptableObject>(dialogueFolderPath, selectedDialogueName);

            dialogueProperty.objectReferenceValue = selectedDialogue;
        }

        private void DrawTriggerParamsArea()
        {
            repeatableProperty.DrawPropertyField();
        }

        private void StopDrawing(string reason, MessageType messageType = MessageType.Info)
        {
            UtilityInspector.DrawHelpBox(reason, messageType);
            UtilityInspector.DrawSpace();
            UtilityInspector.DrawHelpBox("You need to select a Dialogue for this component to work properly at Runtime!", MessageType.Warning);
            serializedObject.ApplyModifiedProperties();
        }

        private void UpdateIndexOnNamesListUpdate(List<string> optionNames, SerializedProperty indexProperty, int oldSelectedPropertyIndex, string oldPropertyName, bool isOldPropertyNull)
        {
            if (isOldPropertyNull)
            {
                indexProperty.intValue = 0;
                return;
            }

            bool oldIndexIsOutOfBoundsOfNamesListCount = oldSelectedPropertyIndex > optionNames.Count - 1;
            bool oldNameIsDifferentThanSelectedName = oldIndexIsOutOfBoundsOfNamesListCount || oldPropertyName != optionNames[oldSelectedPropertyIndex];

            if (oldNameIsDifferentThanSelectedName)
            {
                if (optionNames.Contains(oldPropertyName))
                {
                    indexProperty.intValue = optionNames.IndexOf(oldPropertyName);
                    return;
                }
                indexProperty.intValue = 0;
            }
        }
    }
}