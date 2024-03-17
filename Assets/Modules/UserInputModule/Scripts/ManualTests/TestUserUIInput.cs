using System;
using System.Collections;
using System.Collections.Generic;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.UserInputModule
{
    public class TestUserUIInput : MonoBehaviour
    {
        [SerializeField] private UserInputController _userInputController;

        private void OnEnable()
        {
            if (_userInputController != null)
            {
                _userInputController.LeftMouseButtonClickedOnUI += TestOnLeftMouseButtonHoldOnUI;
            }
        }

        private void OnDisable()
        {
            if (_userInputController != null)
            {
                _userInputController.LeftMouseButtonClickedOnUI -= TestOnLeftMouseButtonHoldOnUI;
            }
        }

        private void TestOnLeftMouseButtonHoldOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            Debug.Log($"TestLog: UI GameObject clicked: {e.GameObject.name}");
        }
    }
}
