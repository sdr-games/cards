namespace SDRGames.Whist.AbilitiesModule.Models
{
    public class AbilityModifier
    {
        public int Value { get; private set; }
        public bool InPercents { get; private set; }

        public AbilityModifier(int value, bool inPercents)
        {
            Value = value;
            InPercents = inPercents;
        }
    }
}
