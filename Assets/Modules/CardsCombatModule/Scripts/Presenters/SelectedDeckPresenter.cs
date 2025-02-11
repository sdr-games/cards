using SDRGames.Whist.CardsCombatModule.Models;
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

        public void SetSelectedDeck(Deck deck)
        {
            _selectedDeckView.SetBacksideImage(deck.Backside);
        }

        public void UnsetSelectedDeck()
        {
            _selectedDeckView.SetBacksideImage(null);
        }
    }
}
