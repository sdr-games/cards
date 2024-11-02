using System;

using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;
using SDRGames.Whist.CardsCombatModule.Presenters;
using SDRGames.Whist.CardsCombatModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class CardManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CardView _cardView;
        [SerializeField] private float _hoverOffset;

        private UserInputController _userInputController;
        private int _siblingIndex;
        private bool _isSelected;

        public CardScriptableObject CardScriptableObject { get; private set; }

        public event EventHandler<CardClickedEventArgs> CardClicked;

        public void Initialize(UserInputController userInputController, Vector3 position, CardScriptableObject cardScriptableObject, int siblingIndex)
        {
            CardScriptableObject = cardScriptableObject;

            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;

            _siblingIndex = siblingIndex;

            new CardPresenter(position, cardScriptableObject, _cardView);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_isSelected)
            {
                _rectTransform.SetAsLastSibling();
                _rectTransform.Translate(Vector3.up * _hoverOffset);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_isSelected)
            {
                _rectTransform.SetSiblingIndex(_siblingIndex);
                _rectTransform.Translate(-Vector3.up * _hoverOffset);
            }
        }

        public void Select()
        {
            _isSelected = true;
        }

        public void Deselect()
        {
            _rectTransform.SetSiblingIndex(_siblingIndex);
            _isSelected = false;
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
            if (_rectTransform == null)
            {
                Debug.LogError("Rect Transform не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_cardView == null)
            {
                Debug.LogError("Card View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}
