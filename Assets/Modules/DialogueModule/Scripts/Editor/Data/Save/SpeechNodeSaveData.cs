using System;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    [Serializable]
    public class SpeechNodeSaveData : BaseNodeSaveData
    {
        [field: SerializeField] public LocalizationSaveData CharacterNameLocalization { get; private set; }
        [field: SerializeField] public LocalizationSaveData TextLocalization { get; private set; }

        public SpeechNodeSaveData(BaseNodeSaveData baseNodeSaveData, LocalizationSaveData characterNameLocalization, LocalizationSaveData textLocalization) : base(baseNodeSaveData.Name, baseNodeSaveData.Answers, baseNodeSaveData.Position)
        {
            SetID(baseNodeSaveData.ID);
            CharacterNameLocalization = characterNameLocalization;
            TextLocalization = textLocalization;
        }

        public void SetID(string id)
        {
            ID = id;
        }

        public override void SetName(string name)
        {
            Name = name;
        }

        public void SetCharacterNameLocalization(LocalizationSaveData characterNameLocalization)
        {
            CharacterNameLocalization = characterNameLocalization;
        }

        public void SetTextLocalization(LocalizationSaveData textLocalization)
        {
            TextLocalization = textLocalization;
        }
    }
}
