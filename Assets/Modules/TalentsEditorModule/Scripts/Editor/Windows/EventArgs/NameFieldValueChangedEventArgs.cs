namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class NameFieldValueChangedEventArgs
    {
        public string Name { get; private set; }

        public NameFieldValueChangedEventArgs(string name)
        {
            Name = name;
        }
    }
}