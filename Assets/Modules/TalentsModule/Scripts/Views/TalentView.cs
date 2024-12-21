using System;
using System.Collections.Generic;

using TMPro;
using SDRGames.Whist.HelpersModule;

using UnityEditor;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDRGames.Whist.TalentsModule.Views
{
    public class TalentView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private List<TalentView> _blockers;
        private List<TalentView> _dependencies;

        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _currentPointsText;
        [SerializeField] private LineView _lineViewPrefab;

        [Header("Tooltip")]
        [SerializeField] private CanvasGroup _tooltipCanvasGroup;
        [SerializeField] private TextMeshProUGUI _tooltipText;

        public event EventHandler BlockChanged;

        public bool IsFilled { get; private set; }
        public bool IsBlocked { get; private set; }

        public void Initialize(int cost, string description, Vector2 position)
        {
            _currentPointsText.text = $"0/{cost}";
            _tooltipText.text = description;

            ((RectTransform)transform).anchoredPosition = position;

            IsFilled = false;
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

        public void ChangeCurrentPoints(string text)
        {
            _currentPointsText.text = text;
        }

        public void SetFilled(bool isFilled)
        {
            IsFilled = isFilled;
            //_image.color = IsActive ? _activeColor : _inactiveColor;
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
                if(!blocker.IsFilled)
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
                SetFilled(false);
                BlockChanged?.Invoke(this, EventArgs.Empty);
            }
            ChangeVisibility(!IsBlocked);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(IsBlocked)
            {
                return;
            }
            _tooltipCanvasGroup.alpha = 1;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsBlocked)
            {
                return;
            }
            _tooltipCanvasGroup.alpha = 0;
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
            this.CheckFieldValueIsNotNull(nameof(_image), _image);
            this.CheckFieldValueIsNotNull(nameof(_button), _button);
            this.CheckFieldValueIsNotNull(nameof(_currentPointsText), _currentPointsText);
            this.CheckFieldValueIsNotNull(nameof(_lineViewPrefab), _lineViewPrefab);

            _blockers = new List<TalentView>();
            _dependencies = new List<TalentView>();
        }
    }
}