using System;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class DeckPreviewClickedEventArgs : EventArgs
    {
        public DeckPreviewManager DeckPreviewManager { get; private set; }

        public DeckPreviewClickedEventArgs(DeckPreviewManager deckPreviewManager)
        {
            DeckPreviewManager = deckPreviewManager;
        }
    }
}