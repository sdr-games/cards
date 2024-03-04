using System;
using UnityEngine;

using static SDRGames.Whist.DialogueSystem.ScriptableObjects.DialogueScriptableObject;
using static SDRGames.Whist.DialogueSystem.ScriptableObjects.DialogueSpeechScriptableObject;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    [Serializable]
    public class SpeechNodeSaveData : BaseNodeSaveData
    {
        [field: SerializeField] public LocalizationSaveData LocalizationSaveData { get; set; }
        [field: SerializeField] public string SelectedLocalizationTable { get; set; }
        [field: SerializeField] public string SelectedLocalizedText { get; set; }
        [field: SerializeField] public string NodeID { get; set; }
        [field: SerializeField] public Quest Quest { get; set; }
        [field: SerializeField] public DialogueQuestActions DialogueQuestAction { get; set; }
        [field: SerializeField] public DialogueActions DialogueAction { get; set; }
    }
}
