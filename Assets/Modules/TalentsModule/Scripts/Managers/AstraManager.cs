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
    public class AstraManager : TalentManager
    {
        private Astra _astra;
        private AstraPresenter _astraPresenter;

        public event EventHandler<AstraChangedEventArgs> AstraChanged;

        public void Initialize(UserInputController userInputController, AstraScriptableObject astraScriptableObject, Vector2 branchSize)
        {
            _astra = new Astra(astraScriptableObject);
            base.Initialize(userInputController, _astra);

            Vector2 position = astraScriptableObject.CalculatePositionInContainer(branchSize - GetSize());
            _astraPresenter = new AstraPresenter(_astra, _talentView, position);

            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
            _userInputController.RightMouseButtonClickedOnUI += OnRightMouseButtonClickedOnUI;
        }

        public override void SetBlock(bool isBlocked, bool silent = false)
        {
            if (_isBlocked == isBlocked)
            {
                return;
            }
            base.SetBlock(isBlocked);

            if (!_isBlocked || silent)
            {
                return;
            }
            AstraChanged?.Invoke(this, new AstraChangedEventArgs(_astra, -_astra.CurrentPoints));
        }

        public override void SetActive(bool isActive, bool silent = false)
        {
            if (_isActive == isActive)
            {
                return;
            }
            base.SetActive(isActive, silent);

            if (_isActive || silent)
            {
                return;
            }
            AstraChanged?.Invoke(this, new AstraChangedEventArgs(_astra, -_astra.CurrentPoints));
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject != gameObject || _astra.CurrentPoints == _astra.TotalCost || _isBlocked)
            {
                return;
            }
            AstraChanged?.Invoke(this, new AstraChangedEventArgs(_astra, 1));
        }

        private void OnRightMouseButtonClickedOnUI(object sender, RightMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject != gameObject || _astra.CurrentPoints == 0 || _isBlocked)
            {
                return;
            }
            AstraChanged?.Invoke(this, new AstraChangedEventArgs(_astra, -1));
        }

        private void OnDisable()
        {
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
            _userInputController.RightMouseButtonClickedOnUI -= OnRightMouseButtonClickedOnUI;
        }
    }
}
