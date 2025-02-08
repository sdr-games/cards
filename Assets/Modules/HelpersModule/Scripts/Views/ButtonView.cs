using System;

using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine.UI;

namespace SDRGames.Whist.HelpersModule.Views
{
    public class ButtonView : Button
    {
        private UserInputController _userInputController;

        public event EventHandler ButtonClicked;

        public void Initialize(UserInputController userInputController, bool interactable = false)
        {
            _userInputController = userInputController;
            if(interactable)
            {
                Activate();
                return;
            }
            Deactivate();
        }

        public void Activate()
        {
            if (interactable)
            {
                return;
            } 
            interactable = true;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
        }

        public void Deactivate()
        {
            if(!interactable)
            {
                return;
            }
            interactable = false;
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if(e.GameObject == gameObject && interactable)
            {
                ButtonClicked?.Invoke(this, EventArgs.Empty);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (_userInputController == null)
            {
                return;
            }
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
        }
    }
}
