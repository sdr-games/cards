using System;
using System.Collections.Generic;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor.Models
{
    [Serializable]
    public class SpeechData : BaseData
    {
        [field: SerializeField] public LocalizationSaveData CharacterNameLocalization { get; private set; }
        [field: SerializeField] public LocalizationSaveData TextLocalization { get; private set; }
        [field: SerializeField] public List<AnswerData> Answers { get; private set; }

        public SpeechData(string name, Vector2 position, LocalizationSaveData characterNameLocalization, LocalizationSaveData textLocalization, List<AnswerData> answers) : base(name, position)
        {
            NodeType = Managers.GraphManager.NodeTypes.Speech;
            CharacterNameLocalization = characterNameLocalization;
            TextLocalization = textLocalization;
            Answers = answers;
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

        public void SetAnswers(List<AnswerData> answers)
        {
            Answers = answers;
        }

        public void AddAnswer(AnswerData answer)
        {
            if (!Answers.Contains(answer))
            {
                Answers.Add(answer);
            }
        }

        public void RemoveAnswer(AnswerData answer)
        {
            if (Answers.Contains(answer))
            {
                Answers.Remove(answer);
            }
        }
    }
}
