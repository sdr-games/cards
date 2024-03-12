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
        [field: SerializeField] public LocalizationData CharacterNameLocalization { get; private set; }
        [field: SerializeField] public LocalizationData TextLocalization { get; private set; }
        [field: SerializeField] public List<AnswerConditionSaveData> Conditions { get; private set; }
        //[field: SerializeField] public List<SpeechData> NextSpeeches { get; private set; }

        public AnswerData(string name, Vector2 position, LocalizationData characterNameLocalization, LocalizationData textLocalization, List<AnswerConditionSaveData> conditions) : base(name, position)
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

        public override void SetNodeName(string name)
        {
            NodeName = name;
        }

        public void SetCharacterNameLocalization(LocalizationData characterNameLocalization)
        {
            CharacterNameLocalization = characterNameLocalization;
        }

        public void SetTextLocalization(LocalizationData textLocalization)
        {
            TextLocalization = textLocalization;
        }

        //public void SetNextSpeeches(List<SpeechData> nextSpeeches)
        //{
        //    NextSpeeches = nextSpeeches;
        //}

        //public void AddNextSpeech(SpeechData nextSpeech)
        //{
        //    if (!NextSpeeches.Contains(nextSpeech))
        //    {
        //        NextSpeeches.Add(nextSpeech);
        //    }
        //}

        //public void RemoveNextSpeech(SpeechData nextSpeech)
        //{
        //    if (NextSpeeches.Contains(nextSpeech))
        //    {
        //        NextSpeeches.Remove(nextSpeech);
        //    }
        //}

        public override void SaveToGraph(GraphSaveDataScriptableObject graphData, Vector2 position)
        {
            SetPosition(position);
            graphData.AnswerNodes.Add(this);
        }

        public DialogueAnswerScriptableObject SaveToSO(DialogueAnswerScriptableObject dialogueSO)
        {
            dialogueSO.Initialize(
                NodeName,
                NodeType,
                CharacterNameLocalization,
                TextLocalization
            );
            return dialogueSO;
        }
    }
}
