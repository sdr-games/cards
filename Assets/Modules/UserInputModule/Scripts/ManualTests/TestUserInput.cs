using System;

using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace SDRGames.Whist.UserInputModule
{
    public class TestUserInput : MonoBehaviour
    {
        [SerializeField] private UserInputController _userInputController;
        [SerializeField] private Key _testKeyCode;

        private void OnEnable()
        {
            if(_userInputController == null)
            {
                Debug.LogError("User Input Controller не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_testKeyCode == Key.None)
            {
                Debug.LogError("Test Key Code не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            _userInputController.KeyboardAnyKeyHold += TestOnKeyHold;
            _userInputController.KeyboardAnyKeyPressed += TestOnKeyPressed;
            _userInputController.KeyboardAnyKeyReleased += TestOnKeyReleased;

            _userInputController.LeftMouseButtonClickedOnScene += TestOnLeftMouseButtonClickedOnScene;
            _userInputController.LeftMouseButtonReleasedOnScene += TestOnLeftMouseButtonReleasedOnScene;

            _userInputController.RightMouseButtonClickedOnScene += TestOnRightMouseButtonClicked;
            _userInputController.RightMouseButtonReleasedOnScene += TestOnRightMouseButtonReleased;

            _userInputController.MiddleMouseButtonScrollStarted += TestOnMiddleMouseButtonScrollStarted;
            _userInputController.MiddleMouseButtonScrollEnded += TestOnMiddleMouseButtonScrollEnded;
        }

        private void OnDisable()
        {
            _userInputController.KeyboardAnyKeyHold -= TestOnKeyHold;
            _userInputController.KeyboardAnyKeyPressed -= TestOnKeyPressed;
            _userInputController.KeyboardAnyKeyReleased -= TestOnKeyReleased;

            _userInputController.LeftMouseButtonClickedOnScene -= TestOnLeftMouseButtonClickedOnScene;
            _userInputController.LeftMouseButtonReleasedOnScene -= TestOnLeftMouseButtonReleasedOnScene;

            _userInputController.RightMouseButtonClickedOnScene -= TestOnRightMouseButtonClicked;
            _userInputController.RightMouseButtonReleasedOnScene -= TestOnRightMouseButtonReleased;

            _userInputController.MiddleMouseButtonScrollStarted -= TestOnMiddleMouseButtonScrollStarted;
            _userInputController.MiddleMouseButtonScrollEnded -= TestOnMiddleMouseButtonScrollEnded;
        }

        private void TestOnMiddleMouseButtonScrollStarted(object sender, MiddleMouseButtonScrollEventArgs e)
        {
            Debug.Log($"TestLog: Scroll on Scroll.x: {e.ScrollPosition.x}, Scroll.y: {e.ScrollPosition.y}");
        }

        private void TestOnMiddleMouseButtonScrollEnded(object sender, MiddleMouseButtonScrollEventArgs e)
        {
            Debug.Log($"TestLog: Scroll ended on Scroll.x: {e.ScrollPosition.x}, Scroll.y: {e.ScrollPosition.y}");
        }

        private void TestOnRightMouseButtonClicked(object sender, RightMouseButtonSceneClickEventArgs e)
        {
            Debug.Log($"TestLog: RMB was pressed on MousePosition.x: {e.MousePosition.x}, MousePosition.y: {e.MousePosition.y}");
        }

        private void TestOnRightMouseButtonReleased(object sender, RightMouseButtonSceneClickEventArgs e)
        {
            Debug.Log($"TestLog: RMB was released on MousePosition.x: {e.MousePosition.x}, MousePosition.y: {e.MousePosition.y}");
        }

        private void TestOnLeftMouseButtonClickedOnScene(object sender, LeftMouseButtonSceneClickEventArgs e)
        {
            Debug.Log($"TestLog: LMB was clicked on: {e.GameObject.name}");
        }

        private void TestOnLeftMouseButtonReleasedOnScene(object sender, LeftMouseButtonSceneClickEventArgs e)
        {
            Debug.Log($"TestLog: LBM was released on: {e.GameObject.name}");
        }

        private void TestOnKeyHold(object sender, EventArgs e)
        {
            //if ((sender as UserInputController).KeyIsPressed(_testKeyCode))
            //{
            //    Debug.Log($"{_testKeyCode} Key was hold");
            //}
            //else
            //{
            //    Debug.LogError($"{_testKeyCode} Key was not hold");
            //}
        }

        private void TestOnKeyPressed(object sender, EventArgs e)
        {
            //if ((sender as UserInputController).KeyWasPressedThisFrame(_testKeyCode))
            //{
            //    Debug.Log($"{_testKeyCode} Key was pressed");
            //}
            //else
            //{
            //    Debug.LogError($"{_testKeyCode} Key was not pressed");
            //}
        }

        private void TestOnKeyReleased(object sender, EventArgs e)
        {
            //if ((sender as UserInputController).KeyWasReleasedThisFrame(_testKeyCode))
            //{
            //    Debug.Log($"{_testKeyCode} Key was released");
            //}
            //else
            //{
            //    Debug.LogError($"{_testKeyCode} Key was not released");
            //}
        }

    }
}
