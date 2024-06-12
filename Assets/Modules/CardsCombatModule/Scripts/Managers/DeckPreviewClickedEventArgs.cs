using System;

using SDRGames.Whist.CardsCombatModule.ScriptableObjects;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class DeckPreviewClickedEventArgs : EventArgs
    {
        public DeckScriptableObject DeckScriptableObject { get; private set; }

        public DeckPreviewClickedEventArgs(DeckScriptableObject deckScriptableObject)
        {
            DeckScriptableObject = deckScriptableObject;
        }
    }
}