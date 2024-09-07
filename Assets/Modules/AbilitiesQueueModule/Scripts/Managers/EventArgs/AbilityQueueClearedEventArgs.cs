namespace SDRGames.Whist.AbilitiesQueueModule.Managers
{
    public class AbilityQueueClearedEventArgs
    {
        public float ReverseAmount { get; private set; }

        public AbilityQueueClearedEventArgs(float reverseAmount)
        {
            ReverseAmount = reverseAmount;
        }
    }
}