using System;
using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.ActiveBlockModule.Models;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.ActiveBlockModule.Views
{
    public class ActiveBlockVariantBUIView : HideableUIView
    {
        [SerializeField] private HideableUIView _wrapperUIView;
        [SerializeField] private RectTransform _indicatorRectTransform;
        [SerializeField] private ActiveBlockSidesModel _sidesModel;
        [SerializeField] private Color _wrongSideColor;
        [SerializeField] private Color _wrongPressIndicatorColor;
        [SerializeField] private Color _correctSideColor;
        [SerializeField] private Color _correctPressIndicatorColor;

        private List<ActiveBlockSideModel> _sides;
        private Coroutine _runCoroutine;

        public event EventHandler<BlockKeyPressedEventArgs> BlockKeyPressed;

        public void Initialize()
        {
            _sides = _sidesModel.GetSides();
        }

        public void RunIndicator(float duration)
        {
            if(_runCoroutine != null)
            {
                StopCoroutine(_runCoroutine);
            }
            _runCoroutine = StartCoroutine(RunIndicatorCoroutine(duration));
        }

        private IEnumerator RunIndicatorCoroutine(float duration)
        {
            _wrapperUIView.Hide();
            int rotationIndex = UnityEngine.Random.Range(0, 3);
            _wrapperUIView.transform.eulerAngles = new Vector3(0, 0, _wrapperUIView.transform.rotation.eulerAngles.z + 90 * rotationIndex);

            ActiveBlockSideModel side;
            float step = Time.deltaTime;
            float sizeChangingSpeed = 0;

            while (_canvasGroup.alpha < 1)
            {
                yield return null;
            }
            yield return null;
            int correctIndex = UnityEngine.Random.Range(1, _sides.Count - 1);
            
            for (int i = 0; i < _sides.Count; i++)
            {
                side = _sides[i];
                if (side == _sidesModel.TopSide || side == _sidesModel.RightSide)
                {
                    sizeChangingSpeed = Mathf.Abs(side.StartOffsetMax.y) / duration * 4 * step;
                }
                else if (side == _sidesModel.BottomSide || side == _sidesModel.LeftSide)
                {
                    sizeChangingSpeed = Mathf.Abs(side.StartOffsetMin.y) / duration * 4 * step;
                }
                side.SideImage.color = _wrongSideColor;
            }
            _sides[correctIndex].SideImage.color = _correctSideColor;
            _wrapperUIView.Show();

            int index = 0;
            float timer = 0;
            Vector3 offsetMax = Vector3.zero;
            Vector3 offsetMin = Vector3.zero;
            side = _sides[index];

            yield return new WaitForSeconds(1);
            while (timer < duration)
            {
                if(timer >= duration / _sides.Count * index)
                {
                    index++;
                    side = _sides[index - 1];
                    _indicatorRectTransform.offsetMax = side.StartOffsetMax;
                    _indicatorRectTransform.offsetMin = side.StartOffsetMin;
                }
                yield return new WaitForSeconds(step);
                if(UserInputController.KeyWasPressedThisFrame("Space") || UserInputController.KeyWasReleasedThisFrame("Space"))
                {
                    side.SideImage.color = index - 1 == correctIndex ? _correctPressIndicatorColor : _wrongPressIndicatorColor;
                    yield return new WaitForSeconds(1);
                    BlockKeyPressed?.Invoke(this, new BlockKeyPressedEventArgs(index - 1 == correctIndex));
                    yield break;
                }
                timer += step;
                if(side == _sidesModel.TopSide)
                {
                    offsetMax = new Vector3(_indicatorRectTransform.offsetMax.x + sizeChangingSpeed, _indicatorRectTransform.offsetMax.y);
                    offsetMin = new Vector3(_indicatorRectTransform.offsetMin.x + sizeChangingSpeed, _indicatorRectTransform.offsetMin.y);
                }
                else if (side == _sidesModel.RightSide)
                {
                    offsetMax = new Vector3(_indicatorRectTransform.offsetMax.x, _indicatorRectTransform.offsetMax.y + sizeChangingSpeed);
                    offsetMin = new Vector3(_indicatorRectTransform.offsetMin.x, _indicatorRectTransform.offsetMin.y + sizeChangingSpeed);
                }
                else if (side == _sidesModel.BottomSide)
                {
                    offsetMax = new Vector3(_indicatorRectTransform.offsetMax.x - sizeChangingSpeed, _indicatorRectTransform.offsetMax.y);
                    offsetMin = new Vector3(_indicatorRectTransform.offsetMin.x - sizeChangingSpeed, _indicatorRectTransform.offsetMin.y);
                }
                else if (side == _sidesModel.LeftSide)
                {
                    offsetMax = new Vector3(_indicatorRectTransform.offsetMax.x, _indicatorRectTransform.offsetMax.y - sizeChangingSpeed);
                    offsetMin = new Vector3(_indicatorRectTransform.offsetMin.x, _indicatorRectTransform.offsetMin.y - sizeChangingSpeed);
                }
                _indicatorRectTransform.offsetMax = offsetMax;
                _indicatorRectTransform.offsetMin = offsetMin;
            }
            BlockKeyPressed?.Invoke(this, new BlockKeyPressedEventArgs(false));
        }
    }
}
