using System;

using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.UserInputModule.Controller;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SDRGames.Whist.ActiveBlockModule.Views
{
    public class StanceSwitcherUIView : HideableUIView, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private LocalizedString _activatedStanceDescription;
        [SerializeField] private LocalizedString _deactivatedStanceDescription;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private float _deactivatedIconAlpha = 0.25f;

        private UserInputController _userInputController;
        private bool _defensiveStanceActivated;

        public event EventHandler<StanceSwitchedEventArgs> StanceSwitched;

        public void Initialize(UserInputController userInputController)
        {
            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
            _canvasGroup.alpha = _deactivatedIconAlpha;
            _defensiveStanceActivated = false;
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject)
            {
                _defensiveStanceActivated = !_defensiveStanceActivated;
                _canvasGroup.alpha = _defensiveStanceActivated ? 1f : _deactivatedIconAlpha;
                _descriptionText.text = _defensiveStanceActivated ? _deactivatedStanceDescription.GetLocalizedText() : _activatedStanceDescription.GetLocalizedText();
                StanceSwitched?.Invoke(this, new StanceSwitchedEventArgs(_defensiveStanceActivated));
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _descriptionText.text = _defensiveStanceActivated ? _deactivatedStanceDescription.GetLocalizedText() : _activatedStanceDescription.GetLocalizedText();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _descriptionText.text = "";
        }
    }                 
}
