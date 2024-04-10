using System;

using static SDRGames.Whist.TalentsEditorModule.Models.TalamusData;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class TalamusLoadedEventArgs : EventArgs
    {
        public CharacteristicNames CharacteristicName { get; private set; }
        public int CharacteristicValue { get; private set; }

        public TalamusLoadedEventArgs(CharacteristicNames characteristicName, int characteristicValue)
        {
            CharacteristicName = characteristicName;
            CharacteristicValue = characteristicValue;
        }
    }
}