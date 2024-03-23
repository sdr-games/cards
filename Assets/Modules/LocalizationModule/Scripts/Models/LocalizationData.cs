using System;

using UnityEngine;
using UnityEngine.Localization;

namespace SDRGames.Whist.LocalizationModule.Models
{
    [Serializable]
    public class LocalizationData
    {
        [field: SerializeField] public string SelectedLocalizationTable { get; private set; }
        [field: SerializeField] public string SelectedEntryKey { get; private set; }
        [field: SerializeField] public string LocalizedTextPreview { get; private set; }

        public LocalizationData(string selectedLocalizationTable, string selectedEntryKey, string localizedTextPreview)
        {
            SelectedLocalizationTable = selectedLocalizationTable;
            SelectedEntryKey = selectedEntryKey;
            LocalizedTextPreview = localizedTextPreview;
        }

        public void SetLocalizationTable(string localizationTableName)
        {
            SelectedLocalizationTable = localizationTableName;
        }

        public void SetEntryKey(string entryKey)
        {
            SelectedEntryKey = entryKey;
        }

        public void SetPreviewText(string text)
        {
            LocalizedTextPreview = text;
        }

        public string GetLocalizedText()
        {
            LocalizedString localizedString = new LocalizedString(SelectedLocalizationTable, SelectedEntryKey);
            return localizedString.GetLocalizedString();
        }
    }
}
