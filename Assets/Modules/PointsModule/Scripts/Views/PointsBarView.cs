using SDRGames.Whist.LocalizationModule.Models;

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
            _pointsValueText.text = $"{currentPointsValue} / {maxPointsValue}";
        }

        #region MonoBehaviour methods
        private void OnEnable()
        {
            if(_background == null)
            {
                Debug.LogError("Background image не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_spentFiller == null)
            {
                Debug.LogError("Filler image не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_pointsValueText == null)
            {
                Debug.LogError("Points Value TextMeshPro не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
        #endregion
    }
}
