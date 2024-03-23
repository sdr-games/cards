namespace SDRGames.Whist.DialogueModule.Managers
{
    public class CharacterVisibleSyncedEventArgs
    {
        public float CurrentTime { get; private set; }

        public CharacterVisibleSyncedEventArgs(float currentTime)
        {
            CurrentTime = currentTime;
        }
    }
}