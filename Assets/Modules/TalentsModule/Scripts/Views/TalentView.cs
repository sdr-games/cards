using System;

using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.TalentsModule.Views
{
    public class TalentView : MonoBehaviour
    {
        private Color _activeColor;
        private Color _inactiveColor;

        [SerializeField] private Image _image;

        public bool IsActive { get; private set; }

        public EventHandler<ActivationChangedEventArgs> ActivationChanged;

        public void Initialize(Color activeColor, Color inactiveColor, Vector2 position)
        {
            _activeColor = activeColor;
            _inactiveColor = inactiveColor;

            _image.color = _inactiveColor;
            transform.position = position;
        }

        public void ChangeActive(bool _isActive)
        {
            IsActive = _isActive;
            _image.color = IsActive ? _activeColor : _inactiveColor;
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
