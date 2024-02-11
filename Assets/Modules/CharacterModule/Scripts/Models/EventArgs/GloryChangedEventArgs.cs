namespace SDRGames.Whist.CharacterModule.Models
{
    public class GloryChangedEventArgs
    {
        public int Glory { get; private set; }

        public GloryChangedEventArgs(int glory)
        {
            Glory = glory;
        }
    }
}