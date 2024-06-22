using System;

using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class DecksListManager : MonoBehaviour
    {
        [SerializeField] private DeckPreviewManager[] _deckPreviewManagers;
        
        private CardsListManager _cardsListManager;
        private DeckScriptableObject[] _decks;

        public DeckScriptableObject SelectedDeck { get; private set; }

        public event EventHandler DeckPreviewClicked;

        public void Initialize(UserInputController userInputController, CardsListManager cardsListManager, DeckScriptableObject[] decks)
        {
            _decks = decks;
            _cardsListManager = cardsListManager;
            for(int i = 0; i < _decks.Length; i++)
            {
                _deckPreviewManagers[i].Initialize(userInputController, _decks[i]);
                _deckPreviewManagers[i].DeckPreviewClicked += OnDeckPreviewClicked;
            }
            SelectedDeck = _decks[0];
            _cardsListManager.Initialize(SelectedDeck.Cards);
        }

        private void OnDeckPreviewClicked(object sender, DeckPreviewClickedEventArgs e)
        {
            SelectedDeck = e.DeckScriptableObject;
            _cardsListManager.Initialize(e.DeckScriptableObject.Cards);
        }

        private void OnEnable()
        {
            if (_deckPreviewManagers.Length == 0)
            {
                Debug.LogError("Deck Preview Managers не были назначены");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }

        private void OnDisable()
        {
            foreach(DeckPreviewManager deckPreviewManager in _deckPreviewManagers)
            {
                deckPreviewManager.DeckPreviewClicked -= OnDeckPreviewClicked;
            }
        }
    }
}
