using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.GlobalMapModule.Views
{
    public class GlobalMapPinView : MonoBehaviour
    {
        private readonly Color DONE_COLOR = new Color(0, 0.8f, 0);
        private readonly Color ACTIVE_COLOR = new Color(0.8f, 0.4f, 0.1f);

        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        [SerializeField] private bool _autoFinish;

        // Available - player pin can be moved to pin position
        // Ready - player pin is on pin position and click will show fight tooltip
        // Done - player pin is on pin position and click will show town tooltip
        // Finished - player pin is on pin position but click has no effect
        public enum Status { Available, Ready, Done, Finished }
        private Status _status;

        public void MarkAsAvailable()
        {
            _image.color = ACTIVE_COLOR;
            _button.interactable = true;
            _status = Status.Available;
        }

        public void MarkAsReady()
        {
            _image.color = ACTIVE_COLOR;
            _button.interactable = true;
            _status = Status.Ready;
        }

        public void MarkAsDone()
        {
            if (_autoFinish)
            {
                MarkAsFinished();
                return;
            }
            _image.color = DONE_COLOR;
            _status = Status.Done;
        }

        public void MarkAsFinished()
        {
            _image.color = DONE_COLOR;
            _button.interactable = false;
            _status = Status.Finished;
        }

        private void OnEnable()
        {
            if (_image == null)
            {
                Debug.LogError("Image не был назначен");
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
    }
}
