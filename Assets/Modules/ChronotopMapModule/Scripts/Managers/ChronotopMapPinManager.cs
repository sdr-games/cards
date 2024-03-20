using SDRGames.Whist.BezierModule.Views;
using SDRGames.Whist.ChronotopMapModule.Controllers;
using SDRGames.Whist.ChronotopMapModule.Models;
using SDRGames.Whist.ChronotopMapModule.Views;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;
using System;
using SDRGames.Whist.UserInputModule.Controller;

namespace SDRGames.Whist.ChronotopMapModule.Managers
{
    public class ChronotopMapPinManager : MonoBehaviour
    {
        [SerializeField] private ChronotopMapFightPinModel _chronotopMapFightPinModel;
        [SerializeField] private ChronotopMapTownPinModel _chronotopMapTownPinModel;
        [SerializeField] private ChronotopMapPinView _chronotopMapPinView;
        [SerializeField] private Button _button;

        [SerializeField] private BezierView _bezierView;
        
        [field: SerializeField] public ChronotopMapPinController ChronotopMapPinController { get; private set; }

        public event EventHandler<AvailablePinClickedEventArgs> AvailablePinClicked;

        public void Initialize(UserInputController userInputController)
        {
            _chronotopMapPinView.Initialize(_button);
            ChronotopMapPinController.Initialize(_chronotopMapPinView, userInputController, _bezierView);
            ChronotopMapPinController.AvailablePinClicked += OnAvailablePinClick;
        }

        private void OnEnable()
        {
            if (_chronotopMapPinView == null)
            {
                Debug.LogError("Chronotop Map Pin View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (ChronotopMapPinController == null)
            {
                Debug.LogError("Chronotop Map Pin Controller не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_button == null)
            {
                Debug.LogError("Button не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }

        private void OnAvailablePinClick(object sender, AvailablePinClickedEventArgs e)
        {
            AvailablePinClicked?.Invoke(this, new AvailablePinClickedEventArgs(e.BezierView, _chronotopMapFightPinModel.DialogueContainerScriptableObject));
        }
    }
}
