using System;

using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.DialogueModule.ScriptableObjects;
using SDRGames.Whist.LocalizationModule.Models;

using UnityEditor;

using UnityEngine;

using static SDRGames.Whist.DialogueModule.ScriptableObjects.DialogueScriptableObject;

namespace SDRGames.Whist.DialogueEditorModule.Models
{
    [Serializable]
    public class AnswerData : BaseData
    {
        [field: SerializeField] public CharacterInfoScriptableObject Character { get; private set; }
        [field: SerializeField] public LocalizationData TextLocalization { get; private set; }

        public AnswerData(string name) : base(name)
        {
            NodeType = NodeTypes.Answer;
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

        public DialogueAnswerScriptableObject SaveToSO(DialogueAnswerScriptableObject dialogueSO)
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
