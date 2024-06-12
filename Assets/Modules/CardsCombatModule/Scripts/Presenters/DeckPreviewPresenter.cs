using System;

using SDRGames.Whist.CardsCombatModule.Views;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Presenters
{
    public class DeckPreviewPresenter
    {
        private DeckPreviewView _deckPreviewView;

        public DeckPreviewPresenter(DeckPreviewView deckPreviewView, ScriptableObject deck)
        {
            _deckPreviewView = deckPreviewView;
            _deckPreviewView.Initialize(null);
        }
    }
}
