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
            interactable = sprite != null;
            if(interactable)
            {
                _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
            }
        }

        public void Unbind()
        {
            SetIconSprite();
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
            AbilitySlotUnbound?.Invoke(this, EventArgs.Empty);
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if(e.GameObject == gameObject && interactable)
            {
                Unbind();
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
