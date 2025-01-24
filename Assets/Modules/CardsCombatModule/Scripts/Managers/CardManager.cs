using System;

using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;
using SDRGames.Whist.CardsCombatModule.Presenters;
using SDRGames.Whist.CardsCombatModule.Views;
using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.HelpersModule;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class CardManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private CardView _cardView;

        private UserInputController _userInputController;
        private int _siblingIndex;
        private bool _isSelected;

        public CardScriptableObject CardScriptableObject { get; private set; }

        public event EventHandler<CardClickedEventArgs> CardClicked;

        public void Initialize(UserInputController userInputController, CardScriptableObject cardScriptableObject, int siblingIndex)
        {
            CardScriptableObject = cardScriptableObject;

            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;

            _siblingIndex = siblingIndex;

            new CardPresenter(cardScriptableObject, _cardView);
        }

        public void SetPosition(Vector2 position)
        {
            _cardView.transform.localPosition = position;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isSelected)
            {
                _cardView.Highlight();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isSelected)
            {
                _cardView.Unhighlight(_siblingIndex);
            }
        }

        public void Select()
        {
            _isSelected = true;
        }

        public void Deselect()
        {
            _isSelected = false;
        }

        public void Destroy()
        {
            Deselect();
            Destroy(gameObject);
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if(e.GameObject == gameObject)
            {
                CardClicked?.Invoke(this, new CardClickedEventArgs(this, _isSelected));
            }
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_cardView), _cardView);
        }

        private void OnDestroy()
        {
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
        }
    }
}
