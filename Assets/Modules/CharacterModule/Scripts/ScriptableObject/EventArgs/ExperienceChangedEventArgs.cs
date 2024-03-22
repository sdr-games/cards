namespace SDRGames.Whist.CharacterModule.ScriptableObjects
{
    public class ExperienceChangedEventArgs
    {
        public int Experience { get; private set; }

        public ExperienceChangedEventArgs(int experience)
        {
            Experience = experience;
        }
    }
}