using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SDRGames.Whist.CardsCombatModule.Views
{
    public class CardView : CardPreviewView, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private float _hoverOffset;

        private int _siblingIndex;

        public void Initialize(Vector3 position, string nameText, string descriptionText, Sprite illustrationSprite, string costText)
        {
            transform.localPosition = position;
            SetRotation();
            transform.SetAsFirstSibling();
            base.Initialize(nameText, descriptionText, illustrationSprite);
            _costText.text = costText;
            _siblingIndex = transform.GetSiblingIndex();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _rectTransform.SetAsLastSibling();
            _rectTransform.Translate(Vector3.up * _hoverOffset);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _rectTransform.SetSiblingIndex(_siblingIndex);
            _rectTransform.Translate(-Vector3.up * _hoverOffset);
        }

        private void SetRotation()
        {
            Vector3 relative = transform.InverseTransformPoint(transform.parent.position);
            float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            transform.RotateAround(transform.TransformPoint(_rectTransform.rect.center), Vector3.forward, 180 - angle);
        }

        private void OnEnable()
        {
            if (_rectTransform == null)
            {
                Debug.LogError("Rect Transform не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_costText == null)
            {
                Debug.LogError("Cost Text не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}
