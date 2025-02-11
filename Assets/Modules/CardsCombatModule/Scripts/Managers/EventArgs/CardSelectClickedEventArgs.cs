using SDRGames.Whist.CardsCombatModule.ScriptableObjects;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class CardSelectClickedEventArgs
    {
        public CardManager CardManager { get; private set; }
        public bool IsSelected { get; private set; }
        public bool MarkedForDisenchant { get; private set; }

        public CardSelectClickedEventArgs(CardManager cardManager, bool isSelected, bool markedForDisenchant)
        {
            CardManager = cardManager;
            IsSelected = isSelected;
            MarkedForDisenchant = markedForDisenchant;
        }
    }
}