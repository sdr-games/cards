using System;
using System.Collections.Generic;
using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor.Models
{
    [Serializable]
    public class AnswerData : BaseData
    {
        [field: SerializeField] public LocalizationSaveData CharacterNameLocalization { get; private set; }
        [field: SerializeField] public LocalizationSaveData TextLocalization { get; private set; }
        [field: SerializeField] public List<AnswerConditionSaveData> Conditions { get; private set; }
        [field: SerializeField] public List<SpeechData> NextSpeeches { get; private set; }

        public AnswerData(string name, Vector2 position, LocalizationSaveData characterNameLocalization, LocalizationSaveData textLocalization, List<AnswerConditionSaveData> conditions) : base(name, position)
        {
            NodeType = Managers.GraphManager.NodeTypes.Answer;
            CharacterNameLocalization = characterNameLocalization;
            TextLocalization = textLocalization;
            Conditions = conditions;
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

        public void SetNextSpeeches(List<SpeechData> nextSpeeches)
        {
            NextSpeeches = nextSpeeches;
        }

        public void AddNextSpeech(SpeechData nextSpeech)
        {
            if (!NextSpeeches.Contains(nextSpeech))
            {
                NextSpeeches.Add(nextSpeech);
            }
        }

        public void RemoveNextSpeech(SpeechData nextSpeech)
        {
            if (NextSpeeches.Contains(nextSpeech))
            {
                NextSpeeches.Remove(nextSpeech);
            }
        }
    }
}
