using System;
using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.BezierModule.Views;
using SDRGames.Whist.ChronotopMapModule.Controllers;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.ChronotopMapModule.Managers
{
    public class ChronotopMapManager : MonoBehaviour
    {
        [SerializeField] private ChronotopMapPinManager[] _chronotopMapPinManagers;
        [SerializeField] private Image _playerPin;
        [SerializeField] private float _playerPinMoveSpeed = 1.0f;

        private Coroutine _playerPinMovementCoroutine;

        private void OnEnable()
        {
            if (_chronotopMapPinManagers.Length == 0)
            {
                Debug.LogError("Chronotop Map Pin Managers не были назначены");
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
                pinManager.Initialize();
                pinManager.ChronotopMapPinController.AvailablePinClicked += MovePlayerPin;
            } 
            //_playerPin.transform.position = _chronotopMapPinManagers[0].transform.position;
        }

        private void MovePlayerPin(object sender, AvailablePinClickedEventArgs e)
        {
            if(_playerPinMovementCoroutine != null)
            {
                return;
            }
            StartCoroutine(MovePlayerPinSmoothlyCoroutine(e.BezierView));
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
            _playerPinMovementCoroutine = null;
        }
    }
}
