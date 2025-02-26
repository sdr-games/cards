using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.HelpersModule;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.CardsCombatModule.Views
{
    public class CardView : CardPreviewView
    {
        private int _siblingIndex;
        private Vector3 _defaultPosition;

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _hoverOffset;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Outline _outline;
        [SerializeField] private Color _hoverOutlineColor;
        [SerializeField] private Color _selectedOutlineColor;
        [SerializeField] private Color _disenchantOutlineColor;

        public void Initialize(int siblingIndex, LocalizedString nameText, string descriptionText, Sprite illustrationSprite, string costText)
        {
            UpdatePositionAndRotation(siblingIndex);

            base.Initialize(nameText, descriptionText, illustrationSprite);
            _costText.text = costText;
        }

        public void UpdatePositionAndRotation(int siblingIndex)
        {
            _siblingIndex = siblingIndex;
            SetRotation();
            transform.SetAsFirstSibling();
            _defaultPosition = transform.localPosition;
        }

        public void HoverHighlight()
        {
            SetHoverPosition();
            _outline.effectColor = _hoverOutlineColor;
            _outline.enabled = true;
        }

        public void HoverUnhighlight()
        {
            SetDefaultPosition();
            _outline.enabled = false;
        }

        public void Pick()
        {
            SetSelectedAndMarkedPosition();
            _outline.effectColor = _selectedOutlineColor;
            _outline.enabled = true;
        }

        public void CancelPick()
        {
            SetDefaultPosition();
            _outline.enabled = false;
        }

        public void MarkForDisenchant()
        {
            SetSelectedAndMarkedPosition();
            _outline.effectColor = _disenchantOutlineColor;
            _outline.enabled = true;
        }

        public void UnmarkForDisenchant()
        {
            SetDefaultPosition();
            _outline.enabled = false;
        }

        private void SetHoverPosition()
        {
            SetDefaultPosition();
            _rectTransform.SetAsLastSibling();
            _rectTransform.Translate(Vector3.up * _hoverOffset);
        }

        private void SetSelectedAndMarkedPosition()
        {
            SetDefaultPosition();
            _rectTransform.SetAsLastSibling();
            _rectTransform.Translate(Vector3.up * _rectTransform.rect.height / 2);
        }

        private void SetDefaultPosition()
        {
            _rectTransform.SetSiblingIndex(_siblingIndex);
            _rectTransform.localPosition = _defaultPosition;
        }

        private void SetRotation()
        {
            Vector3 relative = transform.InverseTransformPoint(transform.parent.position);
            float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            transform.RotateAround(transform.TransformPoint(_rectTransform.rect.center), Vector3.forward, 180 - angle);
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_rectTransform), _rectTransform);
            this.CheckFieldValueIsNotNull(nameof(_costText), _costText);
        }
    }
}
