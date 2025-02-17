namespace SDRGames.Whist.SettingsModule.Views
{
    public class HotkeyChangeSettingsEventArgs
    {
        public string Value { get; private set; }

        public HotkeyChangeSettingsEventArgs(string value)
        {
            Value = value;
        }
    }
}