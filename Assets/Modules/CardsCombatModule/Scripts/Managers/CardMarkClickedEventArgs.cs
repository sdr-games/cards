namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class CardMarkClickedEventArgs
    {
        public CardManager CardManager { get; private set; }
        public bool MarkedForDisenchant { get; private set; }
        public bool IsSelected { get; private set; }

        public CardMarkClickedEventArgs(CardManager cardManager, bool markedForDisenchant, bool isSelected)
        {
            CardManager = cardManager;
            MarkedForDisenchant = markedForDisenchant;
            IsSelected = isSelected;
        }
    }
}