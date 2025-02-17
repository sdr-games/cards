namespace SDRGames.Whist.CharacterModule.ScriptableObjects
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