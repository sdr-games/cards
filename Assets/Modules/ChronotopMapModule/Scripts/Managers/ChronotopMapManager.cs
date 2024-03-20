using System;
using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.BezierModule.Views;
using SDRGames.Whist.ChronotopMapModule.Controllers;
using SDRGames.Whist.DialogueSystem.Managers;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.ChronotopMapModule.Managers
{
    public class ChronotopMapManager : MonoBehaviour
    {
        [SerializeField] private ChronotopMapPinManager[] _chronotopMapPinManagers;
        [SerializeField] private DialogueLinearManager _dialogueManager;
        [SerializeField] private UserInputController _userInputController;
        [SerializeField] private Image _playerPin;
        [SerializeField] private float _playerPinMoveSpeed = 1.0f;

        private BezierView _currentBezierView;
        private int _currentPinIndex;

        private void OnEnable()
        {
            if (_chronotopMapPinManagers.Length == 0)
            {
                Debug.LogError("Chronotop Map Pin Managers не были назначены");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_dialogueManager == null)
            {
                Debug.LogError("Dialogue Container Prefab не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_playerPin == null)
            {
                Debug.LogError("Player Pin не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            foreach (var pinManager in _chronotopMapPinManagers)
            {
                pinManager.Initialize(_userInputController);
            }

            _currentPinIndex = 0;
            _chronotopMapPinManagers[_currentPinIndex].ChronotopMapPinController.MarkAsAvailable();
            _chronotopMapPinManagers[_currentPinIndex].AvailablePinClicked += OnAvailablePinClick;
        }

        private void OnDisable()
        {
            foreach (var pinManager in _chronotopMapPinManagers)
            {
                pinManager.AvailablePinClicked -= OnAvailablePinClick;
            }
        }

        private void OnAvailablePinClick(object sender, AvailablePinClickedEventArgs e)
        {
            if (e.DialogueContainerScriptableObject != null)
            {
                _dialogueManager.Initialize(e.DialogueContainerScriptableObject, _userInputController);
                _dialogueManager.CharacterVisible += OnDialogueCharacterVisible;
                _currentBezierView = e.BezierView;
            }
            else
            {
                StartCoroutine(MovePlayerPinSmoothlyCoroutine(e.BezierView));
            }
            ((ChronotopMapPinManager)sender).AvailablePinClicked -= OnAvailablePinClick;
        }

        private void OnDialogueCharacterVisible(object sender, CharacterVisibleEventArgs e)
        {
            if (e.CurrentTime > 0.99f)
            {
                _playerPin.transform.position = _currentBezierView.GetControlPointsPosition(1);
                _dialogueManager.CharacterVisible -= OnDialogueCharacterVisible;
                _chronotopMapPinManagers[_currentPinIndex].ChronotopMapPinController.MarkAsReady();
                return;
            }
            _playerPin.transform.position = _currentBezierView.GetControlPointsPosition(e.CurrentTime);
        }

        private IEnumerator MovePlayerPinSmoothlyCoroutine(BezierView bezierView)
        {
            float timer = 0;
            yield return null;

            while (timer < 1)
            {
                timer += Time.deltaTime * _playerPinMoveSpeed;
                _playerPin.transform.position = bezierView.GetControlPointsPosition(timer);
                yield return null;
            }
            ChangeCurrentPin();
        }

        private void ChangeCurrentPin()
        {
            _chronotopMapPinManagers[_currentPinIndex].ChronotopMapPinController.MarkAsDone();
            _currentPinIndex++;
            if(_currentPinIndex >= _chronotopMapPinManagers.Length)
            {
                return;
            } 
            _chronotopMapPinManagers[_currentPinIndex].ChronotopMapPinController.MarkAsAvailable();
            _chronotopMapPinManagers[_currentPinIndex].AvailablePinClicked += OnAvailablePinClick;
        }
    }
}
