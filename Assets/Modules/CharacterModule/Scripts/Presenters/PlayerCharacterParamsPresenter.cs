using SDRGames.Whist.PointsModule.Presenters;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Views;

namespace SDRGames.Whist.CharacterModule.Presenters
{
    public class PlayerCharacterParamsPresenter
    {
        private PlayerCharacterParamsModel _playerCharacterParams;
        private PlayerCharacterParamsView _playerCharacterParamsView;

        public PlayerCharacterParamsPresenter(PlayerCharacterParamsModel playerCharacterParams, PlayerCharacterParamsView playerCharacterParamsView)
        {
            _playerCharacterParams = playerCharacterParams;
            _playerCharacterParamsView = playerCharacterParamsView;

            _playerCharacterParamsView.Initialize(
                _playerCharacterParams.Level.ToString(), 
                _playerCharacterParams.Experience.ToString(),
                _playerCharacterParams.Strength.ToString(),
                _playerCharacterParams.Agility.ToString(),
                _playerCharacterParams.Stamina.ToString(),
                _playerCharacterParams.Intelligence.ToString(),
                _playerCharacterParams.PhysicalDamage.ToString(),
                _playerCharacterParams.PhysicalHitChance.ToString(),
                _playerCharacterParams.MagicalDamage.ToString(),
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
            _playerCharacterParams.PhysicalDamageChanged += OnPhysicalDamageChanged;
            _playerCharacterParams.MagicalDamageChanged += OnMagicDamageChanged;
        }

        ~PlayerCharacterParamsPresenter()
        {
            _playerCharacterParams.LevelChanged -= OnLevelChanged;
            _playerCharacterParams.MagicalDamageChanged -= OnMagicDamageChanged;
        }

        private void OnLevelChanged(object sender, LevelChangedEventArgs e)
        {
            _playerCharacterParamsView.SetLevelText(e.Level.ToString());
        }

        private void OnMagicDamageChanged(object sender, ParameterChangedEventArgs e)
        {
            _playerCharacterParamsView.SetMagicalDamageText(e.ParameterValue.ToString());
        }

        private void OnPhysicalDamageChanged(object sender, ParameterChangedEventArgs e)
        {
            _playerCharacterParamsView.SetPhysicalDamageText(e.ParameterValue.ToString());
        }
    }
}
