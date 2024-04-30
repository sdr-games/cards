using System;
using System.Collections;

using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Views
{
    public class BranchView : MonoBehaviour
    {
        public static readonly Vector2 PADDING = new Vector2(50, 50);
        public static Vector2 SIZE { get => GetBranchSize(); }

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;

        private bool _isZoomed;
        private UserInputController _userInputController;

        public event EventHandler<BranchZoomedEventArgs> BranchZoomed;
        public event EventHandler<BranchZoomedEventArgs> BranchUnzoomed;

        public void Initialize(UserInputController userInputController, Vector3 position, float startScale, Transform parent)
        {
            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
            _userInputController.RightMouseButtonClickedOnUI += OnRightMouseButtonClickedOnUI;

            transform.localPosition = position;
            _rectTransform.sizeDelta = SIZE;
            transform.localScale = new Vector2(startScale, startScale);
            transform.SetParent(parent, false);
            SetRotation();
        }

        public void SetSizeSmoothly(float scale, float speed)
        {
            StartCoroutine(SetSizeSmoothlyCoroutine(scale, speed));
        }

        public IEnumerator SetSizeSmoothlyCoroutine(float scale, float speed)
        {
            yield return null;
            Vector3 scaleSpeedVector = Vector3.one * speed * Time.deltaTime;
            Debug.Log(scaleSpeedVector);
            float direction = transform.localScale.x < scale ? 1 : -1;
            while(Math.Abs(transform.localScale.x - scale) > scaleSpeedVector.x && Math.Abs(transform.localScale.y - scale) > scaleSpeedVector.y) 
            {
                yield return null;
                transform.localScale += scaleSpeedVector * direction;
                Debug.Log(transform.localScale);
            }
            transform.localScale = new Vector3(scale, scale, scale);


            //0 -> 76 0.5f
            //0.36 -> 1
        }

        public void Show()
        {
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1;
        }

        public void Hide()
        {
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0;
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

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject && !_isZoomed)
            {
                _isZoomed = true;
                BranchZoomed?.Invoke(this, new BranchZoomedEventArgs(transform.eulerAngles.z));
            }
        }

        private void OnRightMouseButtonClickedOnUI(object sender, RightMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject && _isZoomed)
            {
                _isZoomed = false;
                BranchUnzoomed?.Invoke(this, new BranchZoomedEventArgs(transform.eulerAngles.z));
            }
        }

        private void OnDestroy()
        {
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
        }
    }
}
