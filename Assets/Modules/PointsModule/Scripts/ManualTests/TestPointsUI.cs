using System;

using SDRGames.Whist.PointsModule.Models;
using SDRGames.Whist.PointsModule.Presenters;
using SDRGames.Whist.PointsModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.PointsModule
{
    public class TestPointsUI : MonoBehaviour
    {
        [SerializeField] private Points _points;
        [SerializeField] private PointsBarView _pointsView;

        [Header("TEST SETUP")]
        [SerializeField] private UserInputController _userInputController;

        [SerializeField] private TMP_InputField _currentPointsIncreaseValue;
        [SerializeField] private Button _increaseCurrentPointsButton;

        [SerializeField] private TMP_InputField _currentPointsDecreaseValue;
        [SerializeField] private Button _decreaseCurrentPointsButton;

        private void Start()
        {
            if (_userInputController == null)
            {
                Debug.LogError("User Input Controller не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_pointsView == null)
            {
                Debug.LogError("Points View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_increaseCurrentPointsButton == null)
            {
                Debug.LogError("Increase Button не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_decreaseCurrentPointsButton == null)
            {
                Debug.LogError("Decrease Button не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_currentPointsIncreaseValue == null)
            {
                Debug.LogError("Increase Input не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_currentPointsDecreaseValue == null)
            {
                Debug.LogError("Decrease Input не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            PointsBarPresenter presenter = new PointsBarPresenter(_points, _pointsView);
            _userInputController.LeftMouseButtonClickedOnUI += OnIncreaseCurrentPointsButtonClicked;
            _userInputController.LeftMouseButtonClickedOnUI += OnDecreaseCurrentPointsButtonClicked;
        }

        private void OnIncreaseCurrentPointsButtonClicked(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if(e.GameObject != _increaseCurrentPointsButton.gameObject)
            {
                return;
            }

            Debug.Log($"PreviousValue:  {_points.CurrentValue}");
            _points.IncreaseCurrentValue(Convert.ToSingle(_currentPointsIncreaseValue));
            Debug.Log($"CurrentValue:  {_points.CurrentValue}");
        }

        private void OnDecreaseCurrentPointsButtonClicked(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject != _decreaseCurrentPointsButton.gameObject)
            {
                return;
            }

            Debug.Log($"PreviousValue:  {_points.CurrentValue}");
            //_points.DecreaseCurrentValue(Convert.ToSingle(_currentPointsDecreaseValue));
            Debug.Log($"CurrentValue:  {_points.CurrentValue}");
        }
    }
}
