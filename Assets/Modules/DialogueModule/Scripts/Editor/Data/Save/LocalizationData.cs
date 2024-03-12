using System;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Models
{
    [Serializable]
    public class LocalizationData
    {
        [field: SerializeField] public string SelectedLocalizationTable { get; set; }
        [field: SerializeField] public string SelectedEntryKey { get; set; }
        [field: SerializeField] public string LocalizedText { get; set; }

        public LocalizationData(string selectedLocalizationTable, string selectedEntryKey, string localizedText)
        {
            SelectedLocalizationTable = selectedLocalizationTable;
            SelectedEntryKey = selectedEntryKey;
            LocalizedText = localizedText;
        }
    }
}
