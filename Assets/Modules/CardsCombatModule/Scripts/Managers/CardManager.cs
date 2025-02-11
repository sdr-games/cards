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
        private bool _isPicked;
        private bool _isMarkedForDisenchant;

        public Card Card { get; private set; }

        public event EventHandler<CardSelectClickedEventArgs> CardPicked;
        public event EventHandler<CardMarkClickedEventArgs> CardMarked;

        public void Initialize(UserInputController userInputController, Card card, int siblingIndex)
        {
            Card = card;

            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
            _userInputController.RightMouseButtonClickedOnUI += OnRightMouseButtonClickedOnUI;

            new CardPresenter(Card, _cardView, siblingIndex);
        }

        public void UpdateView(Vector2 position, int siblingIndex)
        {
            _cardView.transform.localPosition = position;
            _cardView.UpdatePositionAndRotation(siblingIndex);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isPicked && !_isMarkedForDisenchant)
            {
                _cardView.HoverHighlight();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isPicked && !_isMarkedForDisenchant)
            {
                _cardView.HoverUnhighlight();
            }
        }

        public void Pick()
        {
            _isPicked = true;
            _isMarkedForDisenchant = false;
            _cardView.Pick();
            if (_isMarkedForDisenchant)
            {
                _cardView.UnmarkForDisenchant();
            }
        }

        public void CancelPick()
        {
            _isPicked = false;
            _cardView.CancelPick();
        }

        public void MarkForDisenchant()
        {
            _isPicked = false;
            _isMarkedForDisenchant = true;
            _cardView.MarkForDisenchant();
            if (_isPicked)
            {
                _cardView.CancelPick();
            }
        }

        public void UnmarkForDisenchant()
        {
            _isMarkedForDisenchant = false;
            _cardView.UnmarkForDisenchant();
        }

        public void Destroy()
        {
            CancelPick();
            UnmarkForDisenchant();
            Destroy(gameObject);
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if(e.GameObject == gameObject)
            {
                CardPicked?.Invoke(this, new CardSelectClickedEventArgs(this, _isPicked, _isMarkedForDisenchant));
            }
        }

        private void OnRightMouseButtonClickedOnUI(object sender, RightMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject)
            {
                CardMarked?.Invoke(this, new CardMarkClickedEventArgs(this, _isMarkedForDisenchant, _isPicked));
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
