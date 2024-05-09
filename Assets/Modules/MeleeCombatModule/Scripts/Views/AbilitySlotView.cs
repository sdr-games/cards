using System;

using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.MeleeCombatModule.Views
{
    public class AbilitySlotView : Button
    {
        private UserInputController _userInputController;

        public event EventHandler AbilitySlotUnbound;

        public void Initialize(UserInputController userInputController)
        {
            _userInputController = userInputController;
            interactable = false;
        }

        public void SetIconSprite(Sprite sprite = null)
        {
            image.sprite = sprite;
            interactable = image.sprite != null;
            if(interactable)
            {
                _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
            }
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if(e.GameObject == gameObject && interactable)
            {
                SetIconSprite();
                _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
                AbilitySlotUnbound?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if(_userInputController == null)
            {
                return;
            }
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
        }
    }
}
