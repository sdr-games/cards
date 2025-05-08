using System;

using SDRGames.Whist.CharacterInfoModule.ScriptableObjects;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.DialogueModule.ScriptableObjects;

using UnityEngine;
using static SDRGames.Whist.DialogueModule.ScriptableObjects.DialogueScriptableObject;
using UnityEditor;

namespace SDRGames.Whist.DialogueEditorModule.Models
{
    [Serializable]
    public class SpeechData : BaseData
    {
        [field: SerializeField] public CharacterInfoScriptableObject Character { get; private set; }
        [field: SerializeField] public LocalizationData TextLocalization { get; private set; }

        public SpeechData(string name) : base(name)
        {
            NodeType = NodeTypes.Speech;
            TextLocalization = new LocalizationData("", "", "");
        }

        public void Load(CharacterInfoScriptableObject character, LocalizationData textLocalization)
        {
            Character = character;
            TextLocalization = textLocalization;
        }

        public override void SetNodeName(string name)
        {
            NodeName = name;
        }

        public void SetCharacter(CharacterInfoScriptableObject character)
        {
            Character = character;
        }

        public void SetTextLocalization(LocalizationData textLocalization)
        {
            TextLocalization = textLocalization;
        }

        public DialogueSpeechScriptableObject SaveToSO(DialogueSpeechScriptableObject dialogueSO)
        {
            dialogueSO.Initialize(
                NodeName,
                NodeType,
                Character,
                TextLocalization
            );
            UtilityIO.SaveAsset(dialogueSO);
            EditorUtility.SetDirty(dialogueSO);
            return dialogueSO;
        }
    }
}
