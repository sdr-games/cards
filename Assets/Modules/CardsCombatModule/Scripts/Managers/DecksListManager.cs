using System;
using System.Collections.Generic;

using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.HelpersModule;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class DecksListManager : MonoBehaviour
    {
        [SerializeField] private DeckPreviewManager _deckPreviewManagerPrefab;
        [SerializeField] private GridLayoutGroup _grid;
        
        private List<DeckPreviewManager> _deckPreviewManagers;
        private CardsListManager _cardsListManager;

        public DeckPreviewManager SelectedDeckPreview { get; private set; }

        public void Initialize(UserInputController userInputController, CardsListManager cardsListManager, DeckScriptableObject[] decksScriptableObject)
        {
            _deckPreviewManagers = new List<DeckPreviewManager>();
            foreach (DeckScriptableObject deckScriptableObject in decksScriptableObject)
            {
                Deck deck = new Deck(deckScriptableObject);
                DeckPreviewManager deckPreviewManager = Instantiate(_deckPreviewManagerPrefab, _grid.transform, false);
                deckPreviewManager.Initialize(userInputController, deck);
                deckPreviewManager.DeckPreviewClicked += OnDeckPreviewClicked;
                _deckPreviewManagers.Add(deckPreviewManager);
            } 
            _cardsListManager = cardsListManager;
            ResetSelection();
        }

        public void ResetSelection()
        {
            SelectedDeckPreview = _deckPreviewManagers[0];
            _cardsListManager.Initialize(SelectedDeckPreview.Deck.Cards);
        }

        public void RemoveSelectedDeckFromList()
        {
            _deckPreviewManagers.Remove(SelectedDeckPreview);
            Destroy(SelectedDeckPreview.gameObject);
        }

        public bool HasAvailableDecks()
        {
            return _deckPreviewManagers.Count > 0;
        }

        private void OnDeckPreviewClicked(object sender, DeckPreviewClickedEventArgs e)
        {
            SelectedDeckPreview = e.DeckPreviewManager;
            _cardsListManager.Initialize(e.DeckPreviewManager.Deck.Cards);
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_deckPreviewManagerPrefab),_deckPreviewManagerPrefab);
            this.CheckFieldValueIsNotNull(nameof(_grid), _grid);
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
