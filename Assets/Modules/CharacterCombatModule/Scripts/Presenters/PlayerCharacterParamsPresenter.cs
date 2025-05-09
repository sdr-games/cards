using SDRGames.Whist.PointsModule.Presenters;
using SDRGames.Whist.CharacterCombatModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Views;
using SDRGames.Whist.CharacterCombatModule.Models;

namespace SDRGames.Whist.CharacterCombatModule.Presenters
{
    public class PlayerCharacterParamsPresenter
    {
        private PlayerParamsScriptableObject _playerCharacterParams;
        private PlayerCharacterParamsView _playerCharacterParamsView;

        public PlayerCharacterParamsPresenter(PlayerParamsScriptableObject playerCharacterParams, PlayerCharacterParamsView playerCharacterParamsView)
        {
            _playerCharacterParams = playerCharacterParams;
            _playerCharacterParamsView = playerCharacterParamsView;

            _playerCharacterParamsView.Initialize(
                _playerCharacterParams.Level.ToString(), 
                _playerCharacterParams.Experience.ToString(),
                CharacterParametersScaling.Instance.ExperienceRequiredPerLevel[_playerCharacterParams.Level - 1].ToString(),
                _playerCharacterParams.Strength.ToString(),
                _playerCharacterParams.Agility.ToString(),
                _playerCharacterParams.Stamina.ToString(),
                _playerCharacterParams.Intelligence.ToString(),
                _playerCharacterParams.PhysicalDamageModifier.ToString(),
                _playerCharacterParams.PhysicalHitChance.ToString(),
                _playerCharacterParams.MagicalDamageModifier.ToString(),
                _playerCharacterParams.MagicalHitChance.ToString(),
                _playerCharacterParams.StaminaRestorationPower.ToString(),
                _playerCharacterParams.Piercing.ToString()
            );

            new PointsTextPresenter(_playerCharacterParams.HealthPoints, _playerCharacterParamsView.HealthPointsView);
            new PointsTextPresenter(_playerCharacterParams.StaminaPoints, _playerCharacterParamsView.StaminaPointsView);
            new PointsTextPresenter(_playerCharacterParams.BreathPoints, _playerCharacterParamsView.BreathPointsView);
            new PointsTextPresenter(_playerCharacterParams.ArmorPoints, _playerCharacterParamsView.ArmorPointsView);
            new PointsTextPresenter(_playerCharacterParams.BarrierPoints, _playerCharacterParamsView.BarrierPointsView);

            _playerCharacterParams.LevelChanged += OnLevelChanged;
            _playerCharacterParams.ExperienceChanged += OnExperienceChanged;
            
            _playerCharacterParams.StrengthChanged += OnStrengthChanged;
            _playerCharacterParams.AgilityChanged += OnAgilityChanged;
            _playerCharacterParams.StaminaChanged += OnStaminaChanged;
            _playerCharacterParams.IntelligenceChanged += OnIntelligenceChanged;

            _playerCharacterParams.PhysicalDamageChanged += OnPhysicalDamageChanged;
            _playerCharacterParams.PhysicalHitChanceChanged += OnPhysicalHitChanceChanged;
            _playerCharacterParams.MagicalDamageChanged += OnMagicalDamageChanged;
            _playerCharacterParams.MagicalHitChanceChanged += OnMagicalHitChanceChanged;
            _playerCharacterParams.StaminaRestorationPowerChanged += OnStaminaRestorationPowerChanged;
            _playerCharacterParams.PiercingChanged += OnPiercingChanged;
        }

        ~PlayerCharacterParamsPresenter()
        {
            _playerCharacterParams.LevelChanged -= OnLevelChanged;
            _playerCharacterParams.ExperienceChanged -= OnExperienceChanged;

            _playerCharacterParams.StrengthChanged -= OnStrengthChanged;
            _playerCharacterParams.AgilityChanged -= OnAgilityChanged;
            _playerCharacterParams.StaminaChanged -= OnStaminaChanged;
            _playerCharacterParams.IntelligenceChanged -= OnIntelligenceChanged;

            _playerCharacterParams.PhysicalDamageChanged -= OnPhysicalDamageChanged;
            _playerCharacterParams.PhysicalHitChanceChanged -= OnPhysicalHitChanceChanged;
            _playerCharacterParams.MagicalDamageChanged -= OnMagicalDamageChanged;
            _playerCharacterParams.MagicalHitChanceChanged -= OnMagicalHitChanceChanged;
            _playerCharacterParams.StaminaRestorationPowerChanged -= OnStaminaRestorationPowerChanged;
            _playerCharacterParams.PiercingChanged -= OnPiercingChanged;
        }

        private void OnLevelChanged(object sender, LevelChangedEventArgs e)
        {
            _playerCharacterParamsView.SetLevelText(e.Level.ToString());
        }

        private void OnExperienceChanged(object sender, ExperienceChangedEventArgs e)
        {
            _playerCharacterParamsView.SetExperienceText(e.CurrentExperience.ToString(), e.RequiredExperience.ToString());
        }

        private void OnStrengthChanged(object sender, CharactersticChangedEventArgs e)
        {
            _playerCharacterParamsView.SetStrengthText(e.CharactersticValue.ToString());
        }

        private void OnAgilityChanged(object sender, CharactersticChangedEventArgs e)
        {
            _playerCharacterParamsView.SetAgilityText(e.CharactersticValue.ToString());
        }

        private void OnStaminaChanged(object sender, CharactersticChangedEventArgs e)
        {
            _playerCharacterParamsView.SetStaminaText(e.CharactersticValue.ToString());
        }

        private void OnIntelligenceChanged(object sender, CharactersticChangedEventArgs e)
        {
            _playerCharacterParamsView.SetIntelligenceText(e.CharactersticValue.ToString());
        }

        private void OnPhysicalDamageChanged(object sender, ParameterChangedEventArgs e)
        {
            _playerCharacterParamsView.SetPhysicalDamageText(e.ParameterValue.ToString());
        }

        private void OnPhysicalHitChanceChanged(object sender, ParameterChangedEventArgs e)
        {
            _playerCharacterParamsView.SetPhysicalHitChanceText(e.ParameterValue.ToString());
        }

        private void OnMagicalDamageChanged(object sender, ParameterChangedEventArgs e)
        {
            _playerCharacterParamsView.SetMagicalDamageText(e.ParameterValue.ToString());
        }

        private void OnMagicalHitChanceChanged(object sender, ParameterChangedEventArgs e)
        {
            _playerCharacterParamsView.SetMagicHitChanceText(e.ParameterValue.ToString());
        }

        private void OnStaminaRestorationPowerChanged(object sender, ParameterChangedEventArgs e)
        {
            _playerCharacterParamsView.SetStaminaRestorationPerRoundText(e.ParameterValue.ToString());
        }

        private void OnPiercingChanged(object sender, ParameterChangedEventArgs e)
        {
            _playerCharacterParamsView.SetPiercingText(e.ParameterValue.ToString());
        }
    }
}
