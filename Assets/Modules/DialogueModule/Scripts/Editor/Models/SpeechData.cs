using SDRGames.Whist.DialogueSystem.Models;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor.Models
{
    public class SpeechData : BaseData
    {
        [field: SerializeField] public LocalizationData CharacterNameLocalization { get; private set; }
        [field: SerializeField] public LocalizationData TextLocalization { get; private set; }

        public SpeechData(string name, Vector2 position, LocalizationData characterNameLocalization, LocalizationData textLocalization) : base(name, position)
        {
            NodeType = Managers.GraphManager.NodeTypes.Speech;
            CharacterNameLocalization = characterNameLocalization;
            TextLocalization = textLocalization;
        }

        public override void SetNodeName(string name)
        {
            NodeName = name;
        }

        public DialogueSpeechScriptableObject SaveToSO(DialogueSpeechScriptableObject dialogueSO)
        {
            dialogueSO.Initialize(
                NodeName,
                NodeType,
                CharacterNameLocalization,
                TextLocalization
            );
            UtilityIO.SaveAsset(dialogueSO);
            return dialogueSO;
        }
    }
}
