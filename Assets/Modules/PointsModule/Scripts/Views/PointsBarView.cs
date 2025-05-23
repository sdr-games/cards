using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.HelpersModule;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.PointsModule.Views
{
    public class PointsBarView : MonoBehaviour
    {
        private readonly Color BACKGROUND_COLOR = Color.black;

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _background;
        [SerializeField] private Image _reservedFiller;
        [SerializeField] private Image _spentFiller;
        [SerializeField] private TextMeshProUGUI _pointsValueText;

        [SerializeField] private Color _reservedFillerColor;
        [SerializeField] private Color _spentFillerColor;

        [field: SerializeField] public LocalizedString ErrorMessage { get; private set; }

        public void Initialize(float maxPointsValue)
        {            
            _background.color = BACKGROUND_COLOR;
            _reservedFiller.color = _reservedFillerColor;
            _spentFiller.color = _spentFillerColor;
            SetMaxPointsText(maxPointsValue, 100);
        }

        public void ChangeSpentFillerValue(float currentValuePercent)
        {
            _spentFiller.fillAmount = currentValuePercent / 100;
        }

        public void ChangeReservedFillerValue(float currentValuePercent)
        {
            _reservedFiller.fillAmount = currentValuePercent / 100;
        }

        public void SetMaxPointsText(float maxPointsValue, float currentValuePercent)
        {
            float currentValue = maxPointsValue * currentValuePercent / 100;
            SetPointsText(currentValue, maxPointsValue);
        }

        public void SetPointsText(float currentPointsValue, float maxPointsValue)
        {
            _pointsValueText.text = $"{(int)currentPointsValue} / {(int)maxPointsValue}";
        }

        public Color GetColor()
        {
            return _spentFillerColor;
        }

        #region MonoBehaviour methods
        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_background), _background);
            this.CheckFieldValueIsNotNull(nameof(_spentFiller), _spentFiller);
            this.CheckFieldValueIsNotNull(nameof(_pointsValueText), _pointsValueText);
        }
        #endregion
    }
}
