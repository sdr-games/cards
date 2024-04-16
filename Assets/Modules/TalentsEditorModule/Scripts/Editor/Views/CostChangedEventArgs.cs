namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class CostChangedEventArgs
    {
        public int Cost { get; private set; }

        public CostChangedEventArgs(int cost)
        {
            Cost = cost;
        }
    }
}