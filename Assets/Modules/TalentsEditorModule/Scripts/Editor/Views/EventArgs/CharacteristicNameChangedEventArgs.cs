using System;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class CharacteristicNameChangedEventArgs : EventArgs
    {
        public string CharacteristicName { get; private set; }

        public CharacteristicNameChangedEventArgs(string characteristicName)
        {
            CharacteristicName = characteristicName;
        }
    }
}