using System;

using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Editor
{
    [Serializable]
    public class LocalizationSaveData
    {
        [field: SerializeField] public string SelectedLocalizationTable { get; set; }
        [field: SerializeField] public string SelectedEntryKey { get; set; }
        [field: SerializeField] public string LocalizedText { get; set; }

        public LocalizationSaveData(string selectedLocalizationTable, string selectedEntryKey, string localizedText)
        {
            SelectedLocalizationTable = selectedLocalizationTable;
            SelectedEntryKey = selectedEntryKey;
            LocalizedText = localizedText;
        }
    }
}
