using System;
using System.Collections;

using SDRGames.Whist.UserInputModule.Controller;

using UnityEditor;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDRGames.Whist.TalentsModule.Views
{
    public class BranchView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private readonly float DEFAULT_ALPHA = 0.6f; // 191/255
        private readonly float HIGHLIGHTED_ALPHA = 1f; // 255/255

        public static readonly Vector2 PADDING = new Vector2(50, 50);

        [SerializeField] private float _speed = 8f;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Image _backgroundImage;

        private bool _isMoving;
        private float _zoomInScale;
        private float _zoomOutScale;
        private UserInputController _userInputController;
        
        public bool IsZoomed { get; private set; }

        public event EventHandler<BranchZoomedEventArgs> BranchZoomInStarted;
        public event EventHandler<BranchZoomedEventArgs> BranchZoomOutStarted;
        public event EventHandler<BranchVisibilityChangedEventArgs> BranchVisibilityChanged;

        public void Initialize(UserInputController userInputController, Vector3 position, float startScale, Transform parent)
        {
            _userInputController = userInputController;
            _userInputController.LeftMouseButtonClickedOnUI += OnLeftMouseButtonClickedOnUI;
            _userInputController.RightMouseButtonClickedOnUI += OnRightMouseButtonClickedOnUI;

            _speed /= 1000;
            _zoomInScale = 1;
            _zoomOutScale = startScale;

            transform.localPosition = position;
            transform.localScale = Vector2.one * startScale;
            transform.SetParent(parent, false);
            SetRotation();
        }

        public void SetBackground(Sprite backgroundImage)
        {
            _backgroundImage.sprite = backgroundImage;
            _rectTransform.sizeDelta = GetBackgroundSize();
        }

        public void SetSizeSmoothly(float scale, float time)
        {
            StartCoroutine(SetSizeSmoothlyCoroutine(scale, time));
        }

        public IEnumerator SetSizeSmoothlyCoroutine(float scale, float time)
        {
            Debug.Log(time);
            yield return null;
            float direction = transform.localScale.x < scale ? 1 : -1;
            Vector3 speedVector = Vector3.one * _speed * direction;
            Debug.Log(speedVector);
            float currentTime = 0;
            while(currentTime < time) 
            {
                yield return _speed;
                transform.localScale += speedVector;
                currentTime++;
            }
            Debug.Log(currentTime);
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(IsZoomed)
            {
                return;
            }
            _backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, HIGHLIGHTED_ALPHA);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (IsZoomed)
            {
                return;
            }
            _backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, DEFAULT_ALPHA);
        }

        public Vector2 GetBackgroundSize()
        {
            return _backgroundImage.sprite.rect.size;
        }

        private void OnLeftMouseButtonClickedOnUI(object sender, LeftMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject && !IsZoomed && !_isMoving)
            {
                IsZoomed = true;
                _isMoving = true;
                _backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, HIGHLIGHTED_ALPHA);
                float angle = transform.localEulerAngles.z > 0 ? 360 - transform.localEulerAngles.z : 0;
                float scalingTime = Math.Abs(transform.localScale.x - _zoomInScale) / _speed;
                BranchZoomInStarted?.Invoke(this, new BranchZoomedEventArgs(angle, scalingTime));
            }
        }

        private void OnRightMouseButtonClickedOnUI(object sender, RightMouseButtonUIClickEventArgs e)
        {
            if (e.GameObject == gameObject && IsZoomed)
            {
                IsZoomed = false;
                _isMoving = true;
                _backgroundImage.color = new Color(_backgroundImage.color.r, _backgroundImage.color.g, _backgroundImage.color.b, DEFAULT_ALPHA);
                float angle = transform.localEulerAngles.z > 0 ? 360 - transform.localEulerAngles.z : 0;
                float scalingTime = Math.Abs(transform.localScale.x - _zoomOutScale) / _speed;
                BranchZoomOutStarted?.Invoke(this, new BranchZoomedEventArgs(transform.localEulerAngles.z - 360, scalingTime));
            }
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
            BranchVisibilityChanged?.Invoke(this, new BranchVisibilityChangedEventArgs(visibility));
        }

        private void OnEnable()
        {
            if(_backgroundImage == null)
            {
                Debug.LogError("Background Image не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_canvasGroup == null)
            {
                Debug.LogError("Canvas Group не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_rectTransform == null)
            {
                Debug.LogError("Rect Transform не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }

        private void OnDestroy()
        {
            _userInputController.LeftMouseButtonClickedOnUI -= OnLeftMouseButtonClickedOnUI;
        }
    }
}
