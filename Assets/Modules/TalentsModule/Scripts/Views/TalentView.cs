using System;
using System.Collections.Generic;

using TMPro;
using SDRGames.Whist.HelpersModule;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDRGames.Whist.TalentsModule.Views
{
    public class TalentView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _currentPointsText;
        [SerializeField] private LineView _lineViewPrefab;

        [Header("Tooltip")]
        [SerializeField] private CanvasGroup _tooltipCanvasGroup;
        [SerializeField] private TextMeshProUGUI _tooltipText;

        public void Initialize(int cost, string description, Vector2 position)
        {
            ((RectTransform)transform).anchoredPosition = position;

            ChangeCurrentPoints($"0/{cost}");
            _tooltipText.text = description;
        }

        public void ChangeCurrentPoints(string text)
        {
            _currentPointsText.text = text;
        }

        public void DrawDependencyLine(Vector2 position)
        {
            LineView line = Instantiate(_lineViewPrefab, transform, false);
            line.transform.SetAsFirstSibling();
            line.Initialize(transform.InverseTransformPoint(transform.position), transform.InverseTransformPoint(position));
        }

        public void ChangeAvailability(bool isActive)
        {
            _image.raycastTarget = isActive;
            _button.interactable = isActive;
            _currentPointsText.alpha = !isActive ? 0 : 1;
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_image), _image);
            this.CheckFieldValueIsNotNull(nameof(_button), _button);
            this.CheckFieldValueIsNotNull(nameof(_currentPointsText), _currentPointsText);
            this.CheckFieldValueIsNotNull(nameof(_lineViewPrefab), _lineViewPrefab);
        }
    }
}