using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.CardsCombatModule.Views;

namespace SDRGames.Whist.CardsCombatModule.Presenters
{
    public class SelectedDeckPresenter
    {
        private SelectedDeckView _selectedDeckView;

        public SelectedDeckPresenter(SelectedDeckView selectedDeckView)
        {
            _selectedDeckView = selectedDeckView;
            _selectedDeckView.Initialize();
        }

        public void SetSelectedDeck(DeckScriptableObject deckScriptableObject)
        {
            _selectedDeckView.SetBacksideImage(deckScriptableObject.Backside);
        }
    }
}
