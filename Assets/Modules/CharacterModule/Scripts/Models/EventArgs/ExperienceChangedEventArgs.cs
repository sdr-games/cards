namespace SDRGames.Whist.CharacterModule.Models
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