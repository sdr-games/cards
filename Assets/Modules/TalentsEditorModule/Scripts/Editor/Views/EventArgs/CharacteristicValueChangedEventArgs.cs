using System;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class CharacteristicValueChangedEventArgs : EventArgs
    {
        public int CharacteristicValue { get; private set; }

        public CharacteristicValueChangedEventArgs(int characteristicValue)
        {
            CharacteristicValue = characteristicValue;
        }
    }
}