using System;
using System.Collections.Generic;

using SDRGames.Whist.TalentsModule.Models;
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

        private bool _isActive;
        private bool _isBlocked;

        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        [SerializeField] private LineView _lineViewPrefab;

        [SerializeField] private List<TalentView> _blockers;
        [SerializeField] private List<TalentView> _dependencies;

        public void Initialize(Color activeColor, Color inactiveColor, Vector2 position)
        {
            _activeColor = activeColor;
            _inactiveColor = inactiveColor;

            _image.color = _inactiveColor;
            transform.position = position;

            _isActive = false;
            ChangeBlock();
        }

        public void SetDependencies(List<TalentView> dependencies)
        {
            _dependencies = dependencies;
            foreach (TalentView dependency in _dependencies)
            {
                dependency.AddBlocker(this);
                LineView line = Instantiate(_lineViewPrefab, transform, false);
                line.transform.SetAsFirstSibling();
                line.Initialize(transform.InverseTransformPoint(transform.position), transform.InverseTransformPoint(dependency.transform.position));
            }
        }

        public void AddBlocker(TalentView blocker)
        {
            if(_blockers.Contains(blocker))
            {
                return;
            }
            _blockers.Add(blocker);
            ChangeBlock();
        }

        public void ChangeActive()
        {
            SetActive(!_isActive);
            foreach (TalentView dependency in _dependencies)
            {
                dependency.ChangeBlock();
            }
        }

        private void SetActive(bool isActive)
        {
            _isActive = isActive;
            _image.color = _isActive ? _activeColor : _inactiveColor;
        }

        private void ChangeBlock()
        {
            _isBlocked = false;
            foreach (TalentView blocker in _blockers)
            {
                if(!blocker._isActive)
                {
                    _isBlocked = true;
                    break;
                }
            }
            if(_isBlocked)
            {
                SetActive(false);
            }
            _button.interactable = !_isBlocked;
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

            if (_lineViewPrefab == null)
            {
                Debug.LogError("Line View Prefab не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}
