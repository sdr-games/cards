using System;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class EquipmentChangedEventArgs : EventArgs
    {
        public string Equipment { get; private set; }

        public EquipmentChangedEventArgs(string equipment)
        {
            Equipment = equipment;
        }
    }
}