namespace SDRGames.Whist.SettingsModule.Views
{
    public class RangeChangeSettingsEventArgs
    {
        public float Value { get; private set; }

        public RangeChangeSettingsEventArgs(float value)
        {
            Value = value;
        }
    }
}