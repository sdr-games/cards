using TMPro;
using UnityEditor;

using UnityEngine;

namespace SDRGames.Islands.PointsModule.Views
{
    public class PointsTextView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _pointsNameText;
        [SerializeField] private TextMeshProUGUI _pointsValueText;

        private float _maxPointsValue;

        public void Initialize(string name, float maxPointsValue)
        {
            _pointsNameText.text = name;
            _maxPointsValue = maxPointsValue;
            SetPointsText(maxPointsValue);
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
            if (_pointsNameText == null)
            {
                Debug.LogError("Points Name TextMeshPro не был назначен");
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
