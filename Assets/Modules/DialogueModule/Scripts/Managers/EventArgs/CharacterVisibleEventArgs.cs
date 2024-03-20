namespace SDRGames.Whist.DialogueSystem.Managers
{
    public class CharacterVisibleEventArgs
    {
        public float CurrentTime { get; private set; }

        public CharacterVisibleEventArgs(float currentTime)
        {
            CurrentTime = currentTime;
        }
    }
}