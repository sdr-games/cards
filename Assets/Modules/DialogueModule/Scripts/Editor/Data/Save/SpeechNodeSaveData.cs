using System;
using System.Collections.Generic;

using UnityEngine;

using static SDRGames.Whist.DialogueSystem.ScriptableObjects.DialogueScriptableObject;
using static SDRGames.Whist.DialogueSystem.ScriptableObjects.DialogueSpeechScriptableObject;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    [Serializable]
    public class SpeechNodeSaveData : BaseNodeSaveData
    {
        [field: SerializeField] public LocalizationSaveData LocalizationSaveData { get; private set; }
        [field: SerializeField] public string SelectedLocalizationTable { get; private set; }
        [field: SerializeField] public string SelectedLocalizedText { get; private set; }
        [field: SerializeField] public string NodeID { get; private set; }
        //[field: SerializeField] public Quest Quest { get; set; }
        //[field: SerializeField] public DialogueQuestActions DialogueQuestAction { get; set; }
        //[field: SerializeField] public DialogueActions DialogueAction { get; set; }

        public SpeechNodeSaveData(BaseNodeSaveData baseNodeSaveData, LocalizationSaveData localizationSaveData) : base(baseNodeSaveData.Name, baseNodeSaveData.Answers, baseNodeSaveData.Position)
        {
            SetID(baseNodeSaveData.ID);
            LocalizationSaveData = localizationSaveData;
        }

        public void SetID(string id)
        {
            ID = id;
        }

        public override void SetName(string name)
        {
            Name = name;
        }

        public void SetLocalizationSaveData(LocalizationSaveData localizationSaveData)
        {
            LocalizationSaveData = localizationSaveData;
        }
    }
}
