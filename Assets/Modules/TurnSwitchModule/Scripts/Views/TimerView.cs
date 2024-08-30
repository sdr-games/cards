using System;
using System.Collections;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.TurnSwitchModule.Views
{
    public class TimerView : MonoBehaviour
    {
        private int _maxTime;

        [SerializeField] private Image _spentFiller;
        [SerializeField] private TextMeshProUGUI _timerText;

        [SerializeField] private Color _spentFillerColor;

        public event EventHandler TimeOver;

        public void Initialize()
        {
            _spentFiller.color = _spentFillerColor;
        }

        public void StartTimer(int time)
        {
            StopTimer();
            _maxTime = time;
            _timerText.text = $"{_maxTime}";
            StartCoroutine(StartTimerCoroutine());
        }

        public void StopTimer()
        {
            StopAllCoroutines();
        }

        private void ChangeSpentFillerValue(float currentTime)
        {
            _spentFiller.fillAmount = currentTime / _maxTime;
        }

        private void SetPointsText(float currentTime)
        {
            _timerText.text = $"{currentTime + 1}";
        }

        private IEnumerator StartTimerCoroutine()
        {
            float currentTime = _maxTime;
            while (currentTime > 0)
            {
                yield return null;
                currentTime -= Time.deltaTime;
                ChangeSpentFillerValue(currentTime);
                SetPointsText((int)currentTime);
            }
            TimeOver?.Invoke(this, EventArgs.Empty);
        }

        #region MonoBehaviour methods
        private void OnEnable()
        {
            if (_spentFiller == null)
            {
                Debug.LogError("Filler image не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_timerText == null)
            {
                Debug.LogError("Timer Value TextMeshPro не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
        #endregion
    }
}
