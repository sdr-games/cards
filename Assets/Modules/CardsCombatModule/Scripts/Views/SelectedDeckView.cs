using System;
using System.Collections;

using UnityEditor;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDRGames.Whist.CardsCombatModule.Views
{
    public class SelectedDeckView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _backsideImage;
        [SerializeField] private Outline _outline;
        [SerializeField] private float _hoverOffset;
        [SerializeField] private float _hoverOffsetSpeed;
        private Vector3 _middlePosition;

        public void Initialize()
        {
            _middlePosition = new Vector3(_rectTransform.anchoredPosition.x, _rectTransform.anchoredPosition.y + _hoverOffset / 2, 0);
        }

        public void SetBacksideImage(Sprite backsideSprite)
        {
            _backsideImage.sprite = backsideSprite;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //_outline.enabled = true;
            //_rectTransform.anchoredPosition = new Vector3(_rectTransform.anchoredPosition.x, _middlePosition.y + _hoverOffset / 2, 0);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //_outline.enabled = false;
            //_rectTransform.anchoredPosition = new Vector3(_rectTransform.anchoredPosition.x, _middlePosition.y - _hoverOffset / 2, 0);
        }        

        private IEnumerator SetOffsetSmoothlyCoroutine(float hoverOffset, float hoverOffsetSpeed)
        {
            yield return null;
            float direction = hoverOffsetSpeed > 0 ? 1 : -1;
            float finalYPosition = (_middlePosition.y + hoverOffset / 2) * direction;
            while(Math.Abs(_rectTransform.anchoredPosition.y - finalYPosition) > 0)
            {
                yield return null;
                _rectTransform.anchoredPosition = new Vector3(_rectTransform.anchoredPosition.x, _rectTransform.anchoredPosition.y + hoverOffsetSpeed, 0);
            }
            _rectTransform.anchoredPosition = new Vector3(_rectTransform.anchoredPosition.x, finalYPosition, 0);
        }

        private void OnEnable()
        {
            if (_rectTransform == null)
            {
                Debug.LogError("Rect Transform не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            if (_backsideImage == null)
            {
                Debug.LogError("Backside Image не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }

            _middlePosition = new Vector3(_rectTransform.anchoredPosition.x, _rectTransform.anchoredPosition.y + _hoverOffset / 2, 0);
        }
    }
}
