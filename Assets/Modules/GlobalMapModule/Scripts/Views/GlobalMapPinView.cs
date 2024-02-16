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

        public void MarkAsAvailable()
        {
            _image.color = ACTIVE_COLOR;
            _button.interactable = true;
        }

        public void MarkAsReady()
        {
            _image.color = ACTIVE_COLOR;
            _button.interactable = true;
        }

        public void MarkAsDone()
        {
            _image.color = DONE_COLOR;
        }

        public void MarkAsFinished()
        {
            _image.color = DONE_COLOR;
            _button.interactable = false;
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
