namespace SDRGames.Whist.CharacterCombatModule.ScriptableObjects
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