using System.Collections.Generic;
using System;

using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;
using System.Linq;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class DeckOnHandsManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CardManager _cardManagerPrefab;
        [SerializeField] private ButtonView _applyButton;
        [SerializeField] private int _maxCardsOnHandsCount = 4;

        [SerializeField] private DeckScriptableObject _deck;

        private UserInputController _userInputController;
        private List<CardManager> _cards;
        [SerializeField] private List<CardManager> _selectedCards;

        public event EventHandler<CardClickedEventArgs> CardClicked;
        public event EventHandler<ApplyButtonClickedEventArgs> ApplyButtonClicked;

        public void Initialize(UserInputController userInputController)
        {
            _cards = new List<CardManager>();
            _selectedCards = new List<CardManager>();

            _userInputController = userInputController;

            _applyButton.Initialize(userInputController);
            _applyButton.ButtonClicked += OnApplyButtonClicked;
        }

        public void SetSelectedDeck(DeckScriptableObject deckScriptableObject)
        {
            _deck = deckScriptableObject;
            for (int i = 0; i < _maxCardsOnHandsCount; i++)
            {
                int index = UnityEngine.Random.Range(0, _deck.Cards.Length - 1);
                Vector2 position = CalculatePositionInRadius(i);
                CardManager cardManager = Instantiate(_cardManagerPrefab, transform, false);
                cardManager.Initialize(_userInputController, position, _deck.Cards[index], _deck.Cards.Length - i - 1);
                cardManager.CardClicked += OnCardClicked;
                _cards.Add(cardManager);
            }
        }

        public void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public void AddSelectedCards(CardManager cardManager)
        {
            if (_selectedCards.Contains(cardManager))
            {
                return;
            }
            _selectedCards.Add(cardManager);
            cardManager.Select();
            SwitchButtonsActivity();
        }

        public bool RemoveSelectedCard(CardManager cardManager)
        {
            if (!_selectedCards.Contains(cardManager))
            {
                return false;
            }
            _selectedCards.Remove(cardManager);
            cardManager.Deselect();
            SwitchButtonsActivity();
            return true;
        }

        private Vector2 CalculatePositionInRadius(int index)
        {
            float radiansOfSeparation = Mathf.PI / 2 / _maxCardsOnHandsCount * (index + 0.5f);
            return new Vector2(Mathf.Cos(radiansOfSeparation) * _rectTransform.sizeDelta.x, Mathf.Sin(radiansOfSeparation) * _rectTransform.sizeDelta.x);
        }

        private void OnApplyButtonClicked(object sender, EventArgs e)
        {
            List<CardManager> selectedCards = new List<CardManager>(_selectedCards);
            float totalCost = _selectedCards.Where(item => item != null).Sum(item => item.CardScriptableObject.Cost);
            ApplyButtonClicked?.Invoke(this, new ApplyButtonClickedEventArgs(totalCost, selectedCards));
        }

        private void OnCardClicked(object sender, CardClickedEventArgs e)
        {
            CardClicked?.Invoke(this, new CardClickedEventArgs(e.CardManager, e.IsSelected));
        }

        private void SwitchButtonsActivity()
        {
            if (_selectedCards.FirstOrDefault(item => item != null))
            {
                _applyButton.Activate();
                return;
            }
            _applyButton.Deactivate();
        }

        private void OnEnable()
        {
            if (_canvasGroup == null)
            {
                Debug.LogError("Canvas Group не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_rectTransform == null)
            {
                Debug.LogError("Rect Transform не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_cardManagerPrefab == null)
            {
                Debug.LogError("Card Manager Prefab не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}
