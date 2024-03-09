//using System.Collections.Generic;

//using SDRGames.Whist.DialogueSystem.Models;
//using SDRGames.Whist.DialogueSystem.ScriptableObjects;
//using SDRGames.Whist.DialogueSystem.Views;

//using UnityEditor;


//namespace SDRGames.Whist.DialogueSystem.Editor
//{
//    [CustomEditor(typeof(Dialogue))]
//    public class Inspector : UnityEditor.Editor
//    {
//        /* Dialogue Scriptable Objects */
//        private SerializedProperty dialogueContainerProperty;
//        private SerializedProperty dialogueProperty;

//        /* Indexes */
//        private SerializedProperty selectedDialogueIndexProperty;

//        private void OnEnable()
//        {
//            dialogueContainerProperty = serializedObject.FindProperty("dialogueContainer");
//            dialogueProperty = serializedObject.FindProperty("dialogue");

//            selectedDialogueIndexProperty = serializedObject.FindProperty("selectedDialogueIndex");
//        }

//        public override void OnInspectorGUI()
//        {
//            serializedObject.Update();
//            DrawDialogueContainerArea();

//            DialogueContainerScriptableObject currentDialogueContainer = (DialogueContainerScriptableObject)dialogueContainerProperty.objectReferenceValue;
//            if (currentDialogueContainer == null)
//            {
//                StopDrawing("Select a Dialogue Container to see the rest of the Inspector.");
//                return;
//            }

//            List<string> dialogueNames = currentDialogueContainer.GetDialogueNames();
//            string dialogueFolderPath = $"Assets/Resources/ScriptableObjectsAssets/Dialogues/{currentDialogueContainer.FileName}/Dialogues";
//            string dialogueInfoMessage = "There are no Dialogues in this Dialogue Container.";

//            if (dialogueNames.Count == 0)
//            {
//                StopDrawing(dialogueInfoMessage);
//                return;
//            }

//            DrawDialogueArea(dialogueNames, dialogueFolderPath);
//            serializedObject.ApplyModifiedProperties();
//        }

//        private void DrawDialogueContainerArea()
//        {
//            UtilityInspector.DrawHeader("Dialogue Container");

//            dialogueContainerProperty.DrawPropertyField();

//            UtilityInspector.DrawSpace();
//        }

//        private void DrawDialogueArea(List<string> dialogueNames, string dialogueFolderPath)
//        {
//            UtilityInspector.DrawHeader("Dialogue");

//            int oldSelectedDialogueIndex = selectedDialogueIndexProperty.intValue;
//            DialogueScriptableObject oldDialogue = (DialogueScriptableObject)dialogueProperty.objectReferenceValue;

//            bool isOldDialogueNull = oldDialogue == null;
//            string oldDialogueName = isOldDialogueNull ? "" : oldDialogue.Name;

//            UpdateIndexOnNamesListUpdate(dialogueNames, selectedDialogueIndexProperty, oldSelectedDialogueIndex, oldDialogueName, isOldDialogueNull);
//            selectedDialogueIndexProperty.intValue = UtilityInspector.DrawPopup("Dialogue", selectedDialogueIndexProperty, dialogueNames.ToArray());

//            string selectedDialogueName = dialogueNames[selectedDialogueIndexProperty.intValue];
//            DialogueScriptableObject selectedDialogue = UtilityIO.LoadAsset<DialogueScriptableObject>(dialogueFolderPath, selectedDialogueName);

//            dialogueProperty.objectReferenceValue = selectedDialogue;
//            UtilityInspector.DrawDisabledFields(() => dialogueProperty.DrawPropertyField());
//        }

//        private void StopDrawing(string reason, MessageType messageType = MessageType.Info)
//        {
//            UtilityInspector.DrawHelpBox(reason, messageType);
//            UtilityInspector.DrawSpace();
//            UtilityInspector.DrawHelpBox("You need to select a Dialogue for this component to work properly at Runtime!", MessageType.Warning);
//            serializedObject.ApplyModifiedProperties();
//        }

//        private void UpdateIndexOnNamesListUpdate(List<string> optionNames, SerializedProperty indexProperty, int oldSelectedPropertyIndex, string oldPropertyName, bool isOldPropertyNull)
//        {
//            if (isOldPropertyNull)
//            {
//                indexProperty.intValue = 0;
//                return;
//            }

//            bool oldIndexIsOutOfBoundsOfNamesListCount = oldSelectedPropertyIndex > optionNames.Count - 1;
//            bool oldNameIsDifferentThanSelectedName = oldIndexIsOutOfBoundsOfNamesListCount || oldPropertyName != optionNames[oldSelectedPropertyIndex];

//            if (oldNameIsDifferentThanSelectedName)
//            {
//                if (optionNames.Contains(oldPropertyName))
//                {
//                    indexProperty.intValue = optionNames.IndexOf(oldPropertyName);
//                    return;
//                }
//                indexProperty.intValue = 0;
//            }
//        }
//    }
//}