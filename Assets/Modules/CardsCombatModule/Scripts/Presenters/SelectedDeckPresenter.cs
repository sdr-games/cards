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
    }
}
