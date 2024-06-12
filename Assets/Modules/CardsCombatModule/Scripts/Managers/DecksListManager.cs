using System;

using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class DecksListManager : MonoBehaviour
    {
        [SerializeField] private DeckPreviewManager[] _deckPreviewManagers;
        
        private CardsListManager _cardsListManager;
        private ScriptableObject[] _decks;

        public event EventHandler DeckPreviewClicked;

        public void Initialize(UserInputController userInputController, CardsListManager cardsListManager, ScriptableObject[] decks)
        {
            _decks = decks;
            _cardsListManager = cardsListManager;
            for(int i = 0; i < _decks.Length; i++)
            {
                _deckPreviewManagers[i].Initialize(userInputController, _decks[i]);
                _deckPreviewManagers[i].DeckPreviewClicked += OnDeckPreviewClicked;
            }
            _cardsListManager.Initialize(_decks); //probably _deck[0].cards
        }

        private void OnDeckPreviewClicked(object sender, EventArgs e)
        {
            _cardsListManager.Initialize(_decks); //probably e.cards
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
