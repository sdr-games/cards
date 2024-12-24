using System;

using SDRGames.Whist.TalentsModule.Models;
using SDRGames.Whist.TalentsModule.Presenters;
using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class AstraManager : TalentManager
    {
        private Astra _astra;
        private AstraPresenter _astraPresenter;

        public event EventHandler<AstraChangedEventArgs> AstraChanged;

        public void Initialize(UserInputController userInputController, AstraScriptableObject astraScriptableObject, Vector2 position)
        {
            _astra = new Astra(astraScriptableObject);
            base.Initialize(userInputController, _astra);

            _astraPresenter = new AstraPresenter(_astra, _talentView, position);

            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
            _userInputController.RightMouseButtonClickedOnUI += OnRightMouseButtonClickedOnUI;
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject != gameObject || _astra.CurrentPoints == _astra.TotalCost || _talentView.IsBlocked)
            {
                return;
            }
            AstraChanged?.Invoke(this, new AstraChangedEventArgs(_astra, 1));
        }

        private void OnRightMouseButtonClickedOnUI(object sender, RightMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject != gameObject || _astra.CurrentPoints == 0 || _talentView.IsBlocked)
            {
                return;
            }
            AstraChanged?.Invoke(this, new AstraChangedEventArgs(_astra, -1));
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
            _userInputController.RightMouseButtonClickedOnUI -= OnRightMouseButtonClickedOnUI;
        }
    }
}
