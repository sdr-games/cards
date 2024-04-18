using System;

using static SDRGames.Whist.TalentsEditorModule.Models.TalamusData;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class TalamusLoadedEventArgs : EventArgs
    {
        public string ID { get; private set; }
        public string NodeName { get; private set; }
        public int Cost { get; private set; }
        public CharacteristicNames CharacteristicName { get; private set; }
        public int CharacteristicValue { get; private set; }

        public TalamusLoadedEventArgs(string id, string nodeName, int cost, CharacteristicNames characteristicName, int characteristicValue)
        {
            ID = id;
            NodeName = nodeName;
            Cost = cost;
            CharacteristicName = characteristicName;
            CharacteristicValue = characteristicValue;
        }
    }
}