using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.ScriptableObjects;
using SDRGames.Whist.DialogueSystem.Views;

using UnityEditor;


namespace SDRGames.Whist.DialogueSystem.Editor
{
    [CustomEditor(typeof(DDialogue))]
    public class Inspector : UnityEditor.Editor
    {
        /* Dialogue Scriptable Objects */
        private SerializedProperty dialogueContainerProperty;
        private SerializedProperty dialogueGroupProperty;
        private SerializedProperty dialogueProperty;

        /* Filters */
        private SerializedProperty groupedDialoguesProperty;
        private SerializedProperty startingDialoguesOnlyProperty;

        /* Indexes */
        private SerializedProperty selectedDialogueGroupIndexProperty;
        private SerializedProperty selectedDialogueIndexProperty;

        private void OnEnable()
        {
            dialogueContainerProperty = serializedObject.FindProperty("dialogueContainer");
            dialogueGroupProperty = serializedObject.FindProperty("dialogueGroup");
            dialogueProperty = serializedObject.FindProperty("dialogue");

            groupedDialoguesProperty = serializedObject.FindProperty("groupedDialogues");
            startingDialoguesOnlyProperty = serializedObject.FindProperty("startingDialoguesOnly");

            selectedDialogueGroupIndexProperty = serializedObject.FindProperty("selectedDialogueGroupIndex");
            selectedDialogueIndexProperty = serializedObject.FindProperty("selectedDialogueIndex");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDialogueContainerArea();

            DialogueContainerScriptableObject currentDialogueContainer = (DialogueContainerScriptableObject)dialogueContainerProperty.objectReferenceValue;

            if (currentDialogueContainer == null)
            {
                StopDrawing("Select a Dialogue Container to see the rest of the Inspector.");

                return;
            }

            DrawFiltersArea();

            bool currentGroupedDialoguesFilter = groupedDialoguesProperty.boolValue;
            bool currentStartingDialoguesOnlyFilter = startingDialoguesOnlyProperty.boolValue;

            List<string> dialogueNames;

            string dialogueFolderPath = $"Assets/Resources/ScriptableObjectsAssets/Dialogues/{currentDialogueContainer.FileName}";

            string dialogueInfoMessage;

            if (currentGroupedDialoguesFilter)
            {
                List<string> dialogueGroupNames = currentDialogueContainer.GetDialogueGroupNames();

                if (dialogueGroupNames.Count == 0)
                {
                    StopDrawing("There are no Dialogue Groups in this Dialogue Container.");

                    return;
                }

                DrawDialogueGroupArea(currentDialogueContainer, dialogueGroupNames);

                DialogueGroupScriptableObject dialogueGroup = (DialogueGroupScriptableObject)dialogueGroupProperty.objectReferenceValue;

                dialogueNames = currentDialogueContainer.GetGroupedDialogueNames(dialogueGroup, currentStartingDialoguesOnlyFilter);

                dialogueFolderPath += $"/Groups/{dialogueGroup.GroupName}/Dialogues";

                dialogueInfoMessage = "There are no" + (currentStartingDialoguesOnlyFilter ? " Starting" : "") + " Dialogues in this Dialogue Group.";
            }
            else
            {
                dialogueNames = currentDialogueContainer.GetUngroupedDialogueNames(currentStartingDialoguesOnlyFilter);

                dialogueFolderPath += "/Global/Dialogues";

                dialogueInfoMessage = "There are no" + (currentStartingDialoguesOnlyFilter ? " Starting" : "") + " Ungrouped Dialogues in this Dialogue Container.";
            }

            if (dialogueNames.Count == 0)
            {
                StopDrawing(dialogueInfoMessage);

                return;
            }

            DrawDialogueArea(dialogueNames, dialogueFolderPath);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawDialogueContainerArea()
        {
            UtilityInspector.DrawHeader("Dialogue Container");

            dialogueContainerProperty.DrawPropertyField();

            UtilityInspector.DrawSpace();
        }

        private void DrawFiltersArea()
        {
            UtilityInspector.DrawHeader("Filters");

            groupedDialoguesProperty.DrawPropertyField();
            startingDialoguesOnlyProperty.DrawPropertyField();

            UtilityInspector.DrawSpace();
        }

        private void DrawDialogueGroupArea(DialogueContainerScriptableObject dialogueContainer, List<string> dialogueGroupNames)
        {
            UtilityInspector.DrawHeader("Dialogue Group");

            int oldSelectedDialogueGroupIndex = selectedDialogueGroupIndexProperty.intValue;

            DialogueGroupScriptableObject oldDialogueGroup = (DialogueGroupScriptableObject)dialogueGroupProperty.objectReferenceValue;

            bool isOldDialogueGroupNull = oldDialogueGroup == null;

            string oldDialogueGroupName = isOldDialogueGroupNull ? "" : oldDialogueGroup.GroupName;

            UpdateIndexOnNamesListUpdate(dialogueGroupNames, selectedDialogueGroupIndexProperty, oldSelectedDialogueGroupIndex, oldDialogueGroupName, isOldDialogueGroupNull);

            selectedDialogueGroupIndexProperty.intValue = UtilityInspector.DrawPopup("Dialogue Group", selectedDialogueGroupIndexProperty, dialogueGroupNames.ToArray());

            string selectedDialogueGroupName = dialogueGroupNames[selectedDialogueGroupIndexProperty.intValue];

            DialogueGroupScriptableObject selectedDialogueGroup = UtilityIO.LoadAsset<DialogueGroupScriptableObject>($"Assets/Resources/ScriptableObjectsAssets/Dialogues/{dialogueContainer.FileName}/Groups/{selectedDialogueGroupName}", selectedDialogueGroupName);

            dialogueGroupProperty.objectReferenceValue = selectedDialogueGroup;

            UtilityInspector.DrawDisabledFields(() => dialogueGroupProperty.DrawPropertyField());

            UtilityInspector.DrawSpace();
        }

        private void DrawDialogueArea(List<string> dialogueNames, string dialogueFolderPath)
        {
            UtilityInspector.DrawHeader("Dialogue");

            int oldSelectedDialogueIndex = selectedDialogueIndexProperty.intValue;

            DialogueScriptableObject oldDialogue = (DialogueScriptableObject)dialogueProperty.objectReferenceValue;

            bool isOldDialogueNull = oldDialogue == null;

            string oldDialogueName = isOldDialogueNull ? "" : oldDialogue.DialogueName;

            UpdateIndexOnNamesListUpdate(dialogueNames, selectedDialogueIndexProperty, oldSelectedDialogueIndex, oldDialogueName, isOldDialogueNull);

            selectedDialogueIndexProperty.intValue = UtilityInspector.DrawPopup("Dialogue", selectedDialogueIndexProperty, dialogueNames.ToArray());

            string selectedDialogueName = dialogueNames[selectedDialogueIndexProperty.intValue];

            DialogueScriptableObject selectedDialogue = UtilityIO.LoadAsset<DialogueScriptableObject>(dialogueFolderPath, selectedDialogueName);

            dialogueProperty.objectReferenceValue = selectedDialogue;

            UtilityInspector.DrawDisabledFields(() => dialogueProperty.DrawPropertyField());
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