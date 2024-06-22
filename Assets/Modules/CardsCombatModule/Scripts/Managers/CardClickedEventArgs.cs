using SDRGames.Whist.CardsCombatModule.ScriptableObjects;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class CardClickedEventArgs
    {
        public CardManager CardManager { get; private set; }
        public bool IsSelected { get; private set; }

        public CardClickedEventArgs(CardManager cardManager, bool isSelected)
        {
            CardManager = cardManager;
            IsSelected = isSelected;
        }
    }
}