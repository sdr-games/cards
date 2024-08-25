namespace SDRGames.Whist.SettingsModule.Views
{
    public class DropdownChangeSettingsEventArgs
    {
        public int Index { get; private set; }
        public string Value { get; private set; }

        public DropdownChangeSettingsEventArgs(int index, string value)
        {
            Index = index;
            Value = value;
        }
    }
}