using System;

using SDRGames.Whist.CardsCombatModule.Presenters;
using SDRGames.Whist.CardsCombatModule.Views;
using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.HelpersModule;

using UnityEngine;
using UnityEngine.EventSystems;
using SDRGames.Whist.CardsCombatModule.Models;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class CardManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CardView _cardView;

        private UserInputController _userInputController;
        private bool _isSelected;
        private bool _markedForDisenchant;

        public Card Card { get; private set; }

        public event EventHandler<CardSelectClickedEventArgs> CardSelectClicked;
        public event EventHandler<CardMarkClickedEventArgs> CardMarkClicked;

        public void Initialize(UserInputController userInputController, Card card, int siblingIndex)
        {
            Card = card;

            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
            _userInputController.RightMouseButtonClickedOnUI += OnRightMouseButtonClickedOnUI;

            new CardPresenter(Card, _cardView, siblingIndex);
        }

        public void SetPosition(Vector2 position)
        {
            _cardView.transform.localPosition = position;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isSelected && !_markedForDisenchant)
            {
                _cardView.HoverHighlight();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isSelected && !_markedForDisenchant)
            {
                _cardView.HoverUnhighlight();
            }
        }

        public void Select()
        {
            _isSelected = true;
            _markedForDisenchant = false;
            _cardView.Select();
            if (_markedForDisenchant)
            {
                _cardView.UnmarkForDisenchant();
            }
        }

        public void Deselect()
        {
            _isSelected = false;
            _cardView.Deselect();
        }

        public void MarkForDisenchant()
        {
            _isSelected = false;
            _markedForDisenchant = true;
            _cardView.MarkForDisenchant();
            if (_isSelected)
            {
                _cardView.Deselect();
            }
        }

        public void UnmarkForDisenchant()
        {
            _markedForDisenchant = false;
            _cardView.UnmarkForDisenchant();
        }

        public void Destroy()
        {
            Deselect();
            UnmarkForDisenchant();
            Destroy(gameObject);
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if(e.GameObject == gameObject)
            {
                CardSelectClicked?.Invoke(this, new CardSelectClickedEventArgs(this, _isSelected, _markedForDisenchant));
            }
        }

        private void OnRightMouseButtonClickedOnUI(object sender, RightMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject)
            {
                CardMarkClicked?.Invoke(this, new CardMarkClickedEventArgs(this, _markedForDisenchant, _isSelected));
            }
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_cardView), _cardView);
        }

        private void OnDestroy()
        {
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
            _userInputController.RightMouseButtonClickedOnUI -= OnRightMouseButtonClickedOnUI;
        }
    }
}
