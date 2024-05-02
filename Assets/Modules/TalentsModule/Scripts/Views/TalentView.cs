using System;
using System.Collections.Generic;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.TalentsModule.Views
{
    public class TalentView : MonoBehaviour
    {
        private Color _activeColor;
        private Color _inactiveColor;

        private List<TalentView> _blockers;
        private List<TalentView> _dependencies;

        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _currentPointsText;
        [SerializeField] private LineView _lineViewPrefab;

        public event EventHandler BlockChanged;

        public bool IsActive { get; private set; }
        public bool IsBlocked { get; private set; }

        public void Initialize(Color activeColor, Color inactiveColor, int cost, Vector2 position)
        {
            _activeColor = activeColor;
            _inactiveColor = inactiveColor;

            _image.color = _inactiveColor;
            _currentPointsText.text = $"0/{cost}";

            ((RectTransform)transform).anchoredPosition = position;

            IsActive = false;
            ChangeBlock();
            ChangeVisibility(false);
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

        public void ChangeActive()
        {
            if(IsBlocked)
            {
                return;
            }
            SetActive(!IsActive);
        }

        public void ChangeCurrentPoints(string text)
        {
            _currentPointsText.text = text;
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
            _image.color = IsActive ? _activeColor : _inactiveColor;
            foreach (TalentView dependency in _dependencies)
            {
                dependency.ChangeBlock();
            }
        }

        public void ChangeVisibility(bool visibility)
        {
            _button.interactable = visibility;
            _currentPointsText.enabled = visibility;
        }

        public void ChangeBlock()
        {
            bool isBlocked = false;
            foreach (TalentView blocker in _blockers)
            {
                if(!blocker.IsActive)
                {
                    isBlocked = true;
                    break;
                }
            }
            SetBlock(isBlocked);
        }

        public void SetBlock(bool isBlocked)
        {
            IsBlocked = isBlocked;
            if (IsBlocked)
            {
                SetActive(false);
                BlockChanged?.Invoke(this, EventArgs.Empty);
            }
            ChangeVisibility(!IsBlocked);
        }

        private void AddBlocker(TalentView blocker)
        {
            if(_blockers.Contains(blocker))
            {
                return;
            }
            _blockers.Add(blocker);
            ChangeBlock();
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

            if (_currentPointsText == null)
            {
                Debug.LogError("Current Points Text не был назначен");
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

            _blockers = new List<TalentView>();
            _dependencies = new List<TalentView>();
        }
    }
}