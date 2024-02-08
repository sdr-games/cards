namespace SDRGames.Whist.CharacterModule.Models
{
    public class LevelChangedEventArgs
    {
        public int Level { get; private set; }

        public LevelChangedEventArgs(int level)
        {
            Level = level;
        }
    }
}