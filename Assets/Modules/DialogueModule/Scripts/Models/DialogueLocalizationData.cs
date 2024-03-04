using System;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Models
{
    [Serializable]
    public class DialogueLocalizationData
    {
        [field: SerializeField] public string SelectedLocalizationTable { get; set; }
        [field: SerializeField] public string SelectedLocalizedText { get; set; }

        public DialogueLocalizationData(string selectedLocalizationTable, string selectedLocalizedText)
        {
            SelectedLocalizationTable = selectedLocalizationTable;
            SelectedLocalizedText = selectedLocalizedText;
        }
    }

}