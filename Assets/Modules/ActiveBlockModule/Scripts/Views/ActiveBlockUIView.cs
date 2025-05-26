using System;
using System.Collections;

using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.ActiveBlockModule.Views
{
    public class ActiveBlockUIView : HideableUIView
    {
        [SerializeField] private float _maxSize;
        [SerializeField] private RectTransform _indicatorRectTransform;
        [SerializeField] private CanvasGroup _indicatorCanvasGroup;
        [SerializeField] private Image _indicatorImage;
        [SerializeField] private Color _wrongIndicatorColor;
        [SerializeField] private Color _wrongPressIndicatorColor;
        [SerializeField] private Color _correctIndicatorColor;
        [SerializeField] private Color _correctPressIndicatorColor;

        private Vector2 _defaultSize;
        private Coroutine _pingsCoroutine;

        public event EventHandler<BlockKeyPressedEventArgs> BlockKeyPressed;

        public void Initialize()
        {
            _defaultSize = _indicatorRectTransform.sizeDelta;
        }

        public void StartIndicatorPing(int pingsCount, float duration)
        {
            if(_pingsCoroutine != null)
            {
                StopCoroutine(_pingsCoroutine);
            }
            _pingsCoroutine = StartCoroutine(IndicatorPingsCoroutine(pingsCount, duration));
        }

        private IEnumerator IndicatorPingsCoroutine(int pingsCount, float duration)
        {
            _indicatorRectTransform.sizeDelta = _defaultSize;
            _indicatorCanvasGroup.alpha = 1;

            float step = Time.deltaTime;
            float sizeChangingSpeed = Mathf.Abs(_maxSize - _indicatorRectTransform.sizeDelta.x) / (duration * 0.8f) * step;
            float disappearanceSpeed = _indicatorCanvasGroup.alpha / (duration * 0.2f) * step;

            while (_canvasGroup.alpha < 1)
            {
                yield return null;
            }
            int correctIndex = UnityEngine.Random.Range(0, pingsCount - 1);
            for (int i = 0; i < pingsCount; i++)
            {
                _indicatorRectTransform.sizeDelta = _defaultSize;
                _indicatorCanvasGroup.alpha = 1;
                _indicatorImage.color = i == correctIndex ? _correctIndicatorColor : _wrongIndicatorColor;

                float timer = 0;
                while (timer < duration)
                {
                    yield return new WaitForSeconds(step);
                    if(UserInputController.KeyWasPressedThisFrame("Space") || UserInputController.KeyWasReleasedThisFrame("Space"))
                    {
                        _indicatorRectTransform.sizeDelta = new Vector2(_maxSize, _maxSize);
                        _indicatorImage.color = i == correctIndex ? _correctPressIndicatorColor : _wrongPressIndicatorColor;
                        _indicatorCanvasGroup.alpha = 1;
                        BlockKeyPressed?.Invoke(this, new BlockKeyPressedEventArgs(i == correctIndex));
                        yield break;
                    }
                    timer += step;
                    if (_indicatorRectTransform.sizeDelta.x < _maxSize && _indicatorRectTransform.sizeDelta.y < _maxSize)
                    {
                        _indicatorRectTransform.sizeDelta = new Vector2(_indicatorRectTransform.sizeDelta.x + sizeChangingSpeed, _indicatorRectTransform.sizeDelta.y + sizeChangingSpeed);
                        continue;
                    }
                    _indicatorRectTransform.sizeDelta = new Vector2(_maxSize, _maxSize);
                    _indicatorCanvasGroup.alpha -= disappearanceSpeed;
                }
                _indicatorCanvasGroup.alpha = 0;
            }
            BlockKeyPressed?.Invoke(this, new BlockKeyPressedEventArgs(false));
        }
    }
}
