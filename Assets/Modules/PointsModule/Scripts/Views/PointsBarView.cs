using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.PointsModule.Views
{
    public class PointsBarView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        [SerializeField] private Image _background;
        [SerializeField] private Image _filler;

        [SerializeField] private Color _backgroundColor;
        [SerializeField] private Color _fillerColor;

        [SerializeField] private TextMeshProUGUI _pointsValueText;

        private float _maxPointsValue;

        public void Initialize(float maxPointsValue)
        {            
            _background.color = _backgroundColor;
            _filler.color = _fillerColor;
            _maxPointsValue = maxPointsValue;
            SetPointsText(maxPointsValue);
        }

        public void ChangeFillerValue(float currentValuePercent)
        {
            _filler.fillAmount = currentValuePercent / 100;
        }

        public void SetMaxPointsText(float maxPointsValue, float currentValuePercent)
        {
            _maxPointsValue = maxPointsValue;
            float currentValue = _maxPointsValue * currentValuePercent / 100;
            SetPointsText(currentValue);
        }

        public void SetPointsText(float currentPointsValue)
        {
            _pointsValueText.text = $"{currentPointsValue} / {_maxPointsValue}";
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

            if (_filler == null)
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
