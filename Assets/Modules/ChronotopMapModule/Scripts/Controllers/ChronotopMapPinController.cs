using System;
using System.Linq;

using SDRGames.Whist.BezierModule.Views;
using SDRGames.Whist.ChronotopMapModule.Views;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.ChronotopMapModule.Controllers
{
    public class ChronotopMapPinController : MonoBehaviour
    {
        private ChronotopMapPinView _chronotopMapPinView;
        private Button _button;
        private BezierView _bezierView;

        [SerializeField] private bool _autofinish = false;

        // Available - player pin can be moved to pin position
        // Ready - player pin is on pin position and click will show fight tooltip
        // Done - player pin is on pin position and click will show town tooltip
        // Finished - player pin is on pin position but click has no effect
        private enum Status { Available, Ready, Done, Finished }
        private Status _status;

        public event EventHandler<AvailablePinClickedEventArgs> AvailablePinClicked;
        public event EventHandler ReadyPinClicked;
        public event EventHandler DonePinClicked;

        public void Initialize(ChronotopMapPinView chronotopMapPinView, Button button, BezierView bezierView)
        {
            _chronotopMapPinView = chronotopMapPinView;
            _button = button;
            _button.onClick.AddListener(PinClicked);
            _bezierView = bezierView;
        }

        public void MarkAsAvailable()
        {
            _status = Status.Available;
            _chronotopMapPinView.MarkAsAvailable();
        }

        public void MarkAsReady()
        {
            _status = Status.Ready;
            _chronotopMapPinView.MarkAsReady();
        }

        public void MarkAsDone()
        {
            if (_autofinish)
            {
                MarkAsFinished();
                return;
            }

            _status = Status.Done;
            _chronotopMapPinView.MarkAsDone();
        }

        public void MarkAsFinished()
        {
            _status = Status.Finished;
            _chronotopMapPinView.MarkAsFinished();
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
                    AvailablePinClicked?.Invoke(this, new AvailablePinClickedEventArgs(_bezierView));
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
