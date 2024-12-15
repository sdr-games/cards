using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.PointsModule.Views;

using TMPro;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Views
{
    public class PlayerCharacterParamsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelValue;
        [SerializeField] private TextMeshProUGUI _experienceValue;

        #region Characteristics

        [Header("Characteristics")]
        [SerializeField] private TextMeshProUGUI _strengthValue;
        [SerializeField] private TextMeshProUGUI _agilityValue;
        [SerializeField] private TextMeshProUGUI _staminaValue;
        [SerializeField] private TextMeshProUGUI _intelligenceValue;

        #endregion

        #region Points

        [field: Header("Points")]
        [field: SerializeField] public PointsTextView HealthPointsView { get; private set; }
        [field: SerializeField] public PointsTextView StaminaPointsView { get; private set; }
        [field: SerializeField] public PointsTextView BreathPointsView { get; private set; }
        [field: SerializeField] public PointsTextView ArmorPointsView { get; private set; }
        [field: SerializeField] public PointsTextView BarrierPointsView { get; private set; }

        #endregion

        #region Parameters

        [Header("Parameters")]
        [SerializeField] private TextMeshProUGUI _physicalDamageValue;
        [SerializeField] private TextMeshProUGUI _physicalHitChanceValue;
        [SerializeField] private TextMeshProUGUI _magicalDamageValue;
        [SerializeField] private TextMeshProUGUI _magicalHitChanceValue;
        [SerializeField] private TextMeshProUGUI _staminaRestorationPerRoundValue;
        [SerializeField] private TextMeshProUGUI _piercingValue;

        #endregion

        public void Initialize(string level, string experience, string strength, string agility, string stamina, string intelligence, string physicalDamage, string physicalHitChance, string magicalDamage, string magicalHitChance, string staminaRestorationPerRound, string piercing)
        {
            SetLevelText(level);
            SetExperienceText(experience);

            SetStrengthText(strength);
            SetAgilityText(agility);
            SetStaminaText(stamina);
            SetIntelligenceText(intelligence);

            SetPhysicalDamageText(physicalDamage);
            SetPhysicalHitChanceText(physicalHitChance);
            SetMagicalDamageText(magicalDamage);
            SetMagicHitChanceText(magicalHitChance);
            SetStaminaRestorationPerRoundText(staminaRestorationPerRound);
            SetPiercingText(piercing);
        }

        #region Setters

        public void SetLevelText(string level)
        {
            _levelValue.text = level;
        }

        public void SetExperienceText(string experience)
        {
            _experienceValue.text = experience;
        }

        public void SetStrengthText(string strength)
        {
            _strengthValue.text = strength;
        }

        public void SetAgilityText(string agility)
        {
            _agilityValue.text = agility;
        }

        public void SetStaminaText(string stamina)
        {
            _staminaValue.text = stamina;
        }

        public void SetIntelligenceText(string intelligence)
        {
            _intelligenceValue.text = intelligence;
        }

        public void SetPhysicalDamageText(string physicalDamage)
        {
            _physicalDamageValue.text = physicalDamage;
        }

        public void SetPhysicalHitChanceText(string physicalHitChance)
        {
            _physicalHitChanceValue.text = physicalHitChance;
        }

        public void SetMagicalDamageText(string magicalDamage)
        {
            _magicalDamageValue.text = magicalDamage;
        }

        public void SetMagicHitChanceText(string magicalHitChance)
        {
            _magicalHitChanceValue.text = magicalHitChance;
        }

        public void SetStaminaRestorationPerRoundText(string staminaRestorationPerRound)
        {
            _staminaRestorationPerRoundValue.text = staminaRestorationPerRound;
        }

        public void SetPiercingText(string piercing)
        {
            _piercingValue.text = piercing;
        }

        #endregion

        #region MonoBehaviour methods

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_levelValue), _levelValue);
            this.CheckFieldValueIsNotNull(nameof(_experienceValue), _experienceValue);

            this.CheckFieldValueIsNotNull(nameof(_strengthValue), _strengthValue);
            this.CheckFieldValueIsNotNull(nameof(_agilityValue), _agilityValue);
            this.CheckFieldValueIsNotNull(nameof(_staminaValue), _staminaValue);
            this.CheckFieldValueIsNotNull(nameof(_intelligenceValue), _intelligenceValue);

            this.CheckFieldValueIsNotNull(nameof(HealthPointsView), HealthPointsView);
            this.CheckFieldValueIsNotNull(nameof(StaminaPointsView), StaminaPointsView);
            this.CheckFieldValueIsNotNull(nameof(BreathPointsView), BreathPointsView);
            this.CheckFieldValueIsNotNull(nameof(ArmorPointsView), ArmorPointsView);
            this.CheckFieldValueIsNotNull(nameof(BarrierPointsView), BarrierPointsView);

            this.CheckFieldValueIsNotNull(nameof(_physicalDamageValue), _physicalDamageValue);
            this.CheckFieldValueIsNotNull(nameof(_physicalHitChanceValue), _physicalHitChanceValue);
            this.CheckFieldValueIsNotNull(nameof(_magicalDamageValue), _magicalDamageValue);
            this.CheckFieldValueIsNotNull(nameof(_magicalHitChanceValue), _magicalHitChanceValue);
            this.CheckFieldValueIsNotNull(nameof(_staminaRestorationPerRoundValue), _staminaRestorationPerRoundValue);
            this.CheckFieldValueIsNotNull(nameof(_piercingValue), _piercingValue);
        }

        #endregion
    }
}
