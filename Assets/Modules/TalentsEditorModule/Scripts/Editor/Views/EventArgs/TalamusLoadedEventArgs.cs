using System;

using SDRGames.Whist.LocalizationModule.Models;

using static SDRGames.Whist.TalentsModule.Models.Talamus;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class TalamusLoadedEventArgs : EventArgs
    {
        public string ID { get; private set; }
        public string NodeName { get; private set; }
        public LocalizationData DescriptionLocalization { get; protected set; }
        public int Cost { get; private set; }
        public CharacteristicNames CharacteristicName { get; private set; }
        public int CharacteristicValue { get; private set; }

        public TalamusLoadedEventArgs(string id, string nodeName, LocalizationData descriptionLocalization, int cost, CharacteristicNames characteristicName, int characteristicValue)
        {
            ID = id;
            NodeName = nodeName;
            DescriptionLocalization = descriptionLocalization;
            Cost = cost;
            CharacteristicName = characteristicName;
            CharacteristicValue = characteristicValue;
        }
    }
}