namespace SDRGames.Whist.CardsCombatModule.Models
{
    public class CardModifier
    {
        public int Value { get; private set; }
        public bool InPercents { get; private set; }

        public CardModifier(int value, bool inPercents)
        {
            Value = value;
            InPercents = inPercents;
        }
    }
}
