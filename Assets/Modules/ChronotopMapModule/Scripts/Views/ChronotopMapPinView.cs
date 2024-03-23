using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.ChronotopMapModule.Views
{
    public class ChronotopMapPinView : MonoBehaviour
    {
        private readonly Color32 DONE_COLOR = new Color32(78, 180, 65, 255);
        private readonly Color32 READY_COLOR = new Color32(204, 102, 24, 255);
        private readonly Color32 ACTIVE_COLOR = new Color32(180, 180, 65, 255);

        private Button _button;
        
        [SerializeField] private Image _image;

        public void Initialize(Button button)
        {
            _button = button;
        }

        public void MarkAsAvailable()
        {
            _image.color = ACTIVE_COLOR;
            _button.interactable = true;
        }

        public void MarkAsReady()
        {
            _image.color = READY_COLOR;
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
        }
    }
}
