using SDRGames.Whist.MeleeCombatModule.Models;
using SDRGames.Whist.HelpersModule;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SDRGames.Whist.MeleeCombatModule.Views
{
    public class MeleeAttackView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Image _iconImage;

        public void Initialize(MeleeAttack meleeAttack)
        {
            _nameText.text = meleeAttack.Name.GetLocalizedText();
            _descriptionText.text = meleeAttack.GetLocalizedDescription();
            _costText.text = meleeAttack.Cost.ToString();
            _iconImage.sprite = meleeAttack.Icon;
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_nameText), _nameText);
            this.CheckFieldValueIsNotNull(nameof(_descriptionText), _descriptionText);
            this.CheckFieldValueIsNotNull(nameof(_costText), _costText);
            this.CheckFieldValueIsNotNull(nameof(_iconImage), _iconImage);
        }

    }
}
