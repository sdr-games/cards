using System;
using System.Collections.Generic;

using SDRGames.Whist.DialogueSystem.Models;
using SDRGames.Whist.DialogueSystem.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor.Models
{
    [Serializable]
    public class AnswerData : BaseData
    {
        [field: SerializeField] public DialogueCharacterScriptableObject Character { get; private set; }
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

        public void SetCharacter(DialogueCharacterScriptableObject character)
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
