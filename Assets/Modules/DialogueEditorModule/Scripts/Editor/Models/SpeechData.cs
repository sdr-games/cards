using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.DialogueModule.ScriptableObjects;

using UnityEngine;
using static SDRGames.Whist.DialogueModule.ScriptableObjects.DialogueScriptableObject;

namespace SDRGames.Whist.DialogueEditorModule.Models
{
    public class SpeechData : BaseData
    {
        [field: SerializeField] public CharacterInfoScriptableObject Character { get; private set; }
        [field: SerializeField] public LocalizationData TextLocalization { get; private set; }

        public SpeechData(string name, Vector2 position, LocalizationData textLocalization) : base(name, position)
        {
            NodeType = NodeTypes.Speech;
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

        public DialogueSpeechScriptableObject SaveToSO(DialogueSpeechScriptableObject dialogueSO)
        {
            dialogueSO.Initialize(
                NodeName,
                NodeType,
                Character,
                TextLocalization
            );
            UtilityIO.SaveAsset(dialogueSO);
            return dialogueSO;
        }
    }
}
