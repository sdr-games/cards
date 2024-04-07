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

        private UserInputController _userInputController;

        public bool IsActive { get; private set; }

        public EventHandler<ActivationChangedEventArgs> ActivationChanged;

        public void Initialize(Color activeColor, Color inactiveColor)
        {
            _activeColor = activeColor;
            _inactiveColor = inactiveColor;

            _image.color = _inactiveColor;
        }

        private void SetActive(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            IsActive = !IsActive;
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
