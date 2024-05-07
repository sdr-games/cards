using System;

using UnityEngine;

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
            UnityEngine.Localization.LocalizedString localizedString = new UnityEngine.Localization.LocalizedString(SelectedLocalizationTable, SelectedEntryKey);
            return localizedString.GetLocalizedString();
        }
    }
}
