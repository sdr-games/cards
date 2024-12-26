using System;
using System.Collections.Generic;

using SDRGames.Whist.TalentsModule.Models;
using SDRGames.Whist.TalentsModule.Presenters;
using SDRGames.Whist.TalentsModule.ScriptableObjects;
using SDRGames.Whist.TalentsModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Managers
{
    public class TalamusManager : TalentManager
    {
        private Talamus _talamus;
        private TalamusPresenter _talamusPresenter;

        public event EventHandler<TalamusChangedEventArgs> TalamusChanged;

        public void Initialize(UserInputController userInputController, TalamusScriptableObject talamusScriptableObject, Vector2 branchSize)
        {
            _talamus = new Talamus(talamusScriptableObject);
            base.Initialize(userInputController, _talamus);

            Vector2 position = talamusScriptableObject.CalculatePositionInContainer(branchSize - GetSize());
            _talamusPresenter = new TalamusPresenter(_talamus, _talentView, position);

            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
            _userInputController.RightMouseButtonClickedOnUI += OnRightMouseButtonClickedOnUI;
        }

        public override void SetBlock(bool isBlocked, bool silent = false)
        {
            if(_isBlocked == isBlocked)
            {
                return;
            }
            base.SetBlock(isBlocked);

            if(!_isBlocked || silent)
            { 
                return; 
            }
            TalamusChanged?.Invoke(this, new TalamusChangedEventArgs(_talamus, -_talamus.CurrentPoints));
        }

        public override void SetActive(bool isActive, bool silent = false)
        {
            if(_isActive == isActive)
            {
                return;
            }
            base.SetActive(isActive, silent);

            if(_isActive || silent)
            {
                return;
            }
            TalamusChanged?.Invoke(this, new TalamusChangedEventArgs(_talamus, -_talamus.CurrentPoints));
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject != gameObject || _talamus.CurrentPoints == _talamus.TotalCost || _isBlocked)
            {
                return;
            }
            TalamusChanged?.Invoke(this, new TalamusChangedEventArgs(_talamus, 1));
        }

        private void OnRightMouseButtonClickedOnUI(object sender, RightMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject != gameObject || _talamus.CurrentPoints == 0 || _isBlocked)
            {
                return;
            }
            TalamusChanged?.Invoke(this, new TalamusChangedEventArgs(_talamus, -1));
        }

        private void OnDisable()
        {
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
            _userInputController.RightMouseButtonClickedOnUI -= OnRightMouseButtonClickedOnUI;
        }
    }
}
