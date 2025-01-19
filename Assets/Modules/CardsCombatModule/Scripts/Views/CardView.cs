using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.HelpersModule;

using TMPro;

using UnityEditor;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.CardsCombatModule.Views
{
    public class CardView : CardPreviewView
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Outline _outline;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private float _hoverOffset;

        public void Initialize(LocalizedString nameText, LocalizedString descriptionText, Sprite illustrationSprite, string costText)
        {
            SetRotation();
            transform.SetAsFirstSibling();
            base.Initialize(nameText, descriptionText, illustrationSprite);
            _costText.text = costText;
        }

        public void Highlight()
        {
            _rectTransform.SetAsLastSibling();
            _rectTransform.Translate(Vector3.up * _hoverOffset);
            _outline.enabled = true;
        }

        public void Unhighlight(int index)
        {
            _rectTransform.SetSiblingIndex(index);
            _rectTransform.Translate(-Vector3.up * _hoverOffset);
            _outline.enabled = false;
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
