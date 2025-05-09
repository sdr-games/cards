using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.HelpersModule;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.CardsCombatModule.Views
{
    public class CardView : CardPreviewView
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _hoverOffset;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Outline _outline;
        [SerializeField] private Color _hoverOutlineColor;
        [SerializeField] private Color _selectedOutlineColor;
        [SerializeField] private Color _disenchantOutlineColor;

        public void Initialize(LocalizedString nameText, string descriptionText, Sprite illustrationSprite, string costText)
        {
            base.Initialize(nameText, descriptionText, illustrationSprite);
            _costText.text = costText;
        }

        public void HoverHighlight()
        {
            _outline.effectColor = _hoverOutlineColor;
            _outline.enabled = true;
        }

        public void HoverUnhighlight()
        {
            _outline.enabled = false;
        }

        public void Pick()
        {
            _outline.effectColor = _selectedOutlineColor;
            _outline.enabled = true;
        }

        public void CancelPick()
        {
            _outline.enabled = false;
        }

        public void MarkForDisenchant()
        {
            _outline.effectColor = _disenchantOutlineColor;
            _outline.enabled = true;
        }

        public void UnmarkForDisenchant()
        {
            _outline.enabled = false;
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_rectTransform), _rectTransform);
            this.CheckFieldValueIsNotNull(nameof(_costText), _costText);
        }
    }
}
