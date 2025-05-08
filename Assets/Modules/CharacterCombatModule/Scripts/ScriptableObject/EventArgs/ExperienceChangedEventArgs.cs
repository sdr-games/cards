namespace SDRGames.Whist.CharacterCombatModule.ScriptableObjects
{
    public class ExperienceChangedEventArgs
    {
        public int CurrentExperience { get; private set; }
        public int RequiredExperience { get; private set; }

        public ExperienceChangedEventArgs(int currentExperience, int requiredExperience)
        {
            CurrentExperience = currentExperience;
            RequiredExperience = requiredExperience;
        }
    }
}