namespace SDRGames.Whist.TalentsModule.Views
{
    public class BranchZoomedEventArgs
    {
        public float Angle { get; private set; }

        public BranchZoomedEventArgs(float angle)
        {
            Angle = angle;
        }
    }
}