using System;

using SDRGames.Whist.GlobalMapModule.Models;
using SDRGames.Whist.GlobalMapModule.Views;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.GlobalMapModule.Controllers
{
    public class GlobalMapPinController : MonoBehaviour
    {
        [SerializeField] private GlobalMapPinView _globalMapPinView;

        [SerializeField] private Button _button;
        [SerializeField] private bool _autofinish = false;

        // Available - player pin can be moved to pin position
        // Ready - player pin is on pin position and click will show fight tooltip
        // Done - player pin is on pin position and click will show town tooltip
        // Finished - player pin is on pin position but click has no effect
        private enum Status { Available, Ready, Done, Finished }
        private Status _status;

        public event EventHandler AvailablePinClicked;
        public event EventHandler ReadyPinClicked;
        public event EventHandler DonePinClicked;

        public void Initialize(GlobalMapPinView globalMapPinView)
        {
            _globalMapPinView = globalMapPinView;
        }

        public void MarkAsAvailable()
        {
            _status = Status.Available;
            _globalMapPinView.MarkAsAvailable();
        }

        public void MarkAsReady()
        {
            _status = Status.Done;
            _globalMapPinView.MarkAsReady();
        }

        public void MarkAsDone()
        {
            _status = Status.Done;
            _globalMapPinView.MarkAsDone();
        }

        public void MarkAsFinished()
        {
            _status = Status.Finished;
            _globalMapPinView.MarkAsFinished();
        }

        private void OnEnable()
        {
            if (_globalMapPinView == null)
            {
                Debug.LogError("Global Map Pin View не был назначен");
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

            _button.onClick.AddListener(PinClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveAllListeners();
        }

        private void PinClicked()
        {
            switch (_status)
            {
                case Status.Available:
                    AvailablePinClicked?.Invoke(this, EventArgs.Empty);
                    break;
                case Status.Ready:
                    ReadyPinClicked?.Invoke(this, EventArgs.Empty); 
                    break;
                case Status.Done:
                    DonePinClicked?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }
    }
}
