using System;

using SDRGames.Whist.TalentsModule.Models;
using SDRGames.Whist.TalentsModule.Presenters;
using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class TalamusManager : TalentManager
    {
        private Talamus _talamus;
        private TalamusPresenter _talamusPresenter;

        public event EventHandler<TalamusChangedEventArgs> TalamusChanged;

        public void Initialize(UserInputController userInputController, TalamusScriptableObject talamusScriptableObject, Vector2 position)
        {
            _talamus = new Talamus(talamusScriptableObject);
            base.Initialize(userInputController, _talamus);

            _talamusPresenter = new TalamusPresenter(_talamus, _talentView, position);

            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
            _userInputController.RightMouseButtonClickedOnUI += OnRightMouseButtonClickedOnUI;
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject != gameObject || _talamus.CurrentPoints == _talamus.TotalCost || _talentView.IsBlocked)
            {
                return;
            }
            _talamus.IncreaseCurrentPoints();
            TalamusChanged?.Invoke(this, new TalamusChangedEventArgs(_talamus, 1));
        }

        private void OnRightMouseButtonClickedOnUI(object sender, RightMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject != gameObject || _talamus.CurrentPoints == 0 || _talentView.IsBlocked)
            {
                return;
            }
            _talamus.DecreaseCurrentPoints();
            TalamusChanged?.Invoke(this, new TalamusChangedEventArgs(_talamus, -1));
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
            _userInputController.RightMouseButtonClickedOnUI -= OnRightMouseButtonClickedOnUI;
        }
    }
}
