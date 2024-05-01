using System;
using System.Collections;

using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;
using UnityEngine.UIElements;

namespace SDRGames.Whist.TalentsModule.Views
{
    public class BranchView : MonoBehaviour
    {
        public static readonly Vector2 PADDING = new Vector2(50, 50);
        public static Vector2 SIZE { get => GetBranchSize(); }

        [SerializeField] private float _speed = 8f;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        private bool _isMoving;
        private bool _isZoomed;
        private float _zoomedScale;
        private float _unzoomedScale;
        private UserInputController _userInputController;

        public event EventHandler<BranchZoomedEventArgs> BranchZoomed;
        public event EventHandler<BranchZoomedEventArgs> BranchUnzoomed;

        public void Initialize(UserInputController userInputController, Vector3 position, float startScale, Transform parent)
        {
            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
            _userInputController.RightMouseButtonClickedOnUI += OnRightMouseButtonClickedOnUI;

            _speed /= 1000;
            _zoomedScale = 1;
            _unzoomedScale = startScale;

            transform.localPosition = position;
            _rectTransform.sizeDelta = SIZE;
            transform.localScale = Vector2.one * startScale;
            transform.SetParent(parent, false);
            SetRotation();
        }

        public void SetSizeSmoothly(float scale)
        {
            StartCoroutine(SetSizeSmoothlyCoroutine(scale));
        }

        public IEnumerator SetSizeSmoothlyCoroutine(float scale)
        {
            yield return null;
            float direction = transform.localScale.x < scale ? 1 : -1;
            Vector3 speedVector = Vector3.one * _speed * direction;
            while(Math.Abs(transform.localScale.x - scale) > _speed && Math.Abs(transform.localScale.y - scale) > _speed) 
            {
                yield return null;
                transform.localScale += speedVector;
            }
            transform.localScale = new Vector3(scale, scale, scale);
            _isMoving = false;
        }

        public void Show()
        {
            StartCoroutine(SwitchVisibilitySmoothlyCoroutine(true));
        }

        public void Hide()
        {
            StartCoroutine(SwitchVisibilitySmoothlyCoroutine(false));
        }

        private static Vector2 GetBranchSize()
        {
            return new Vector2(Screen.width / 2 + PADDING.x, Screen.height / 2 + PADDING.y);
        }

        private void SetRotation()
        {
            Vector3 relative = transform.InverseTransformPoint(transform.parent.position);
            float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            transform.RotateAround(transform.TransformPoint(_rectTransform.rect.center), Vector3.forward, 180 - angle);
        }

        private IEnumerator SwitchVisibilitySmoothlyCoroutine(bool visibility)
        {
            yield return null;
            if(visibility)
            {
                while(_canvasGroup.alpha < 1)
                {
                    yield return null;
                    _canvasGroup.alpha += _speed * 2;
                }
                _canvasGroup.alpha = 1;
                _canvasGroup.blocksRaycasts = true;
                _canvasGroup.interactable = true;
            }
            else
            {
                _canvasGroup.blocksRaycasts = false;
                _canvasGroup.interactable = false;
                while (_canvasGroup.alpha > 0)
                {
                    yield return null;
                    _canvasGroup.alpha -= _speed * 10;
                }
                _canvasGroup.alpha = 0;
            }
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject && !_isZoomed && !_isMoving)
            {
                _isZoomed = true;
                _isMoving = true;
                float direction = transform.localRotation.w > 0 ? 1 : -1;
                float scalingTime = Math.Abs(transform.localScale.x - _zoomedScale) / _speed;
                BranchZoomed?.Invoke(this, new BranchZoomedEventArgs(transform.localEulerAngles.z * direction, scalingTime));
            }
        }

        private void OnRightMouseButtonClickedOnUI(object sender, RightMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject && _isZoomed)
            {
                _isZoomed = false;
                _isMoving = true;
                float direction = transform.localRotation.w > 0 ? 1 : -1;
                float scalingTime = Math.Abs(transform.localScale.x - _unzoomedScale) / _speed;
                BranchUnzoomed?.Invoke(this, new BranchZoomedEventArgs(-transform.localEulerAngles.z * direction, scalingTime));
            }
        }

        private void OnDestroy()
        {
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
        }
    }
}
