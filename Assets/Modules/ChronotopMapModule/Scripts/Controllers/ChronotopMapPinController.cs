using System;

using SDRGames.Whist.BezierModule.Views;
using SDRGames.Whist.ChronotopMapModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.ChronotopMapModule.Controllers
{
    public class ChronotopMapPinController : MonoBehaviour
    {
        private UserInputController _userInputController; 
        private ChronotopMapPinView _chronotopMapPinView;
        private BezierView _bezierView;

        // Available - player pin can be moved to pin position
        // Ready - player pin is on pin position and click will show fight tooltip
        // Done - player pin is on pin position and click will show town tooltip
        // Finished - player pin is on pin position but click has no effect
        private enum Status { Available, Ready, Done, Finished }
        private Status _status;

        public event EventHandler<AvailablePinClickedEventArgs> AvailablePinClicked;
        public event EventHandler ReadyPinClicked;
        public event EventHandler DonePinClicked;

        public void Initialize(ChronotopMapPinView chronotopMapPinView, UserInputController userInputController, BezierView bezierView)
        {
            _chronotopMapPinView = chronotopMapPinView;
            _userInputController = userInputController;
            _bezierView = bezierView;
        }

        public void MarkAsAvailable()
        {
            _status = Status.Available;
            _userInputController.LeftMouseButtonClickedOnUI += PinClicked;
        }

        public void MarkAsReady()
        {
            _status = Status.Ready;
        }

        public void MarkAsDone()
        {
            _status = Status.Done;
        }

        public void MarkAsFinished()
        {
            _userInputController.LeftMouseButtonClickedOnUI -= PinClicked;
            _status = Status.Finished;
        }

        private void OnDisable()
        {
            _userInputController.LeftMouseButtonClickedOnUI -= PinClicked;
        }

        private void PinClicked(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if(e.GameObject != gameObject)
            {
                return;
            }

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
