using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.ActiveBlockModule.Models;
using SDRGames.Whist.HelpersModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.ActiveBlockModule.Views
{
    public class ActiveBlockUIView : HideableUIView
    {
        [SerializeField] private HideableUIView _wrapperUIView;
        [SerializeField] private ActiveBlockSidesModel _sidesModel;
        [SerializeField] private float _delayAfterKeyPress;
        [SerializeField] private Color _defaultSideColor;
        [SerializeField] private Color _wrongPressIndicatorColor;
        [SerializeField] private Color _correctSideColor;
        [SerializeField] private Color _correctPressIndicatorColor;

        private List<ActiveBlockSideModel> _sides;
        private Coroutine _runCoroutine;

        public event EventHandler<BlockKeyPressedEventArgs> BlockKeyPressed;

        public void Initialize()
        {
            _sides = _sidesModel.GetSides();
            foreach(ActiveBlockSideModel side in _sides)
            {
                side.SetDefaultPosition(side.SideImage.transform.position);
            }
        }

        public void RunIndicator(float durationPerSide)
        {
            if (_runCoroutine != null)
            {
                StopCoroutine(_runCoroutine);
            }
            _runCoroutine = StartCoroutine(RunIndicatorCoroutine(durationPerSide));
        }

        private IEnumerator RunIndicatorCoroutine(float durationPerSide)
        {
            _wrapperUIView.Hide();

            ActiveBlockSideModel side;
            float step = Time.deltaTime;
            int horizontalRotation = UnityEngine.Random.Range(0, 100) > 50 ? 180 : 0;
            int verticalRotation = UnityEngine.Random.Range(0, 100) > 50 ? 180 : 0;
            if(horizontalRotation > 0)
            {
                _sidesModel.TopSide.SideImage.transform.parent.position = _sidesModel.BottomSide.DefaultPosition;
                _sidesModel.BottomSide.SideImage.transform.parent.position = _sidesModel.TopSide.DefaultPosition;
            }
            if (verticalRotation > 0)
            {
                _sidesModel.LeftSide.SideImage.transform.parent.position = _sidesModel.RightSide.DefaultPosition;
                _sidesModel.RightSide.SideImage.transform.parent.position = _sidesModel.LeftSide.DefaultPosition;
            }
            for (int i = 0; i < _sides.Count; i++)
            {
                _sides[i].SideImage.color = _defaultSideColor;
            }

            while (_canvasGroup.alpha < 1)
            {
                yield return null;
            }
            yield return null;
            List<ActiveBlockSideModel> sides = _sides.OrderBy(_ => Guid.NewGuid()).ToList();
            _wrapperUIView.Show();

            float totalDamageMultiplier = 1;
            yield return new WaitForSeconds(1);
            for(int i = 0; i < sides.Count; i++)
            {
                yield return new WaitForSeconds(_delayAfterKeyPress);
                bool correctButtonPressed = false;
                float timer = 0;
                side = sides[i];
                side.SideImage.color = _correctSideColor;
                while (timer < durationPerSide)
                {
                    yield return new WaitForSeconds(step);
                    timer += step;
                    if (UserInputController.AnyKeyWasPressed())
                    {
                        if (UserInputController.KeyIsPressed(side.CorrectKey.ToString()) || UserInputController.KeyWasPressedThisFrame(side.CorrectKey.ToString()) || UserInputController.KeyWasReleasedThisFrame(side.CorrectKey.ToString()))
                        {
                            correctButtonPressed = true;
                            side.SideImage.color = _correctPressIndicatorColor;
                            totalDamageMultiplier -= 0.25f;
                        }
                        break;
                    }
                }
                if(!correctButtonPressed)
                {
                    side.SideImage.color = _wrongPressIndicatorColor;
                }
            }
            yield return new WaitForSeconds(1);
            BlockKeyPressed?.Invoke(this, new BlockKeyPressedEventArgs(totalDamageMultiplier));
        }

        private void OnValidate()
        {
            if (_delayAfterKeyPress < 0.15f)
            {
                _delayAfterKeyPress = 0.15f;
            }
        }
    }
}
