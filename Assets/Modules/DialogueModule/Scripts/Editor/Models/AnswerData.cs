using System;

using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.DialogueModule.ScriptableObjects;
using SDRGames.Whist.LocalizationModule.Models;

using UnityEngine;

namespace SDRGames.Whist.DialogueModule.Editor.Models
{
    [Serializable]
    public class AnswerData : BaseData
    {
        [field: SerializeField] public CharacterInfoScriptableObject Character { get; private set; }
        [field: SerializeField] public LocalizationData TextLocalization { get; private set; }

        public AnswerData(string name, Vector2 position, LocalizationData textLocalization) : base(name, position)
        {
            NodeType = Managers.GraphManager.NodeTypes.Answer;
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

        public DialogueAnswerScriptableObject SaveToSO(DialogueAnswerScriptableObject dialogueSO)
        {
            dialogueSO.Initialize(
                NodeName,
                NodeType,
                Character,
                TextLocalization
            );
            return dialogueSO;
        }
    }
}
