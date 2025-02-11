using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.CardsCombatModule.Views;

namespace SDRGames.Whist.CardsCombatModule.Presenters
{
    public class DeckPreviewPresenter
    {
        private DeckPreviewView _deckPreviewView;

        public DeckPreviewPresenter(DeckPreviewView deckPreviewView, Deck deck)
        {
            _deckPreviewView = deckPreviewView;
            _deckPreviewView.Initialize(deck.Backside);
        }
    }
}
