using System;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Models
{
    [Serializable]
    public class LocalizationData
    {
        [field: SerializeField] public string SelectedLocalizationTable { get; private set; }
        [field: SerializeField] public string SelectedEntryKey { get; private set; }
        [field: SerializeField] public string LocalizedText { get; private set; }

        public LocalizationData(string selectedLocalizationTable, string selectedEntryKey, string localizedText)
        {
            SelectedLocalizationTable = selectedLocalizationTable;
            SelectedEntryKey = selectedEntryKey;
            LocalizedText = localizedText;
        }

        public void SetLocalizationTable(string localizationTableName)
        {
            SelectedLocalizationTable = localizationTableName;
        }

        public void SetEntryKey(string entryKey)
        {
            SelectedEntryKey = entryKey;
        }

        public void SetText(string text)
        {
            LocalizedText = text;
        }
    }
}
