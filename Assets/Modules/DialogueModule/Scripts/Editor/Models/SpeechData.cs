using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.DialogueSystem.Models;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor.Models
{
    public class SpeechData : BaseData
    {
        [field: SerializeField] public CharacterInfoScriptableObject Character { get; private set; }
        [field: SerializeField] public LocalizationData TextLocalization { get; private set; }

        public SpeechData(string name, Vector2 position, LocalizationData textLocalization) : base(name, position)
        {
            NodeType = Managers.GraphManager.NodeTypes.Speech;
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
