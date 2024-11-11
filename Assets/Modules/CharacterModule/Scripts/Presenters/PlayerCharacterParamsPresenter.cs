using SDRGames.Whist.DiceModule.Presenters;
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
                _playerCharacterParams.CharacterInfo.CharacterNameLocalization.GetLocalizedString(), 
                _playerCharacterParams.Level.ToString(), 
                _playerCharacterParams.Experience.ToString(),
                _playerCharacterParams.Glory.ToString(),
                _playerCharacterParams.PhysicalDamage.ToString(),
                _playerCharacterParams.MagicDamage.ToString()
            );

            new PointsTextPresenter(_playerCharacterParams.HealthPoints, _playerCharacterParamsView.HealthPointsView);
            new PointsTextPresenter(_playerCharacterParams.StaminaPoints, _playerCharacterParamsView.StaminaPointsView);
            new PointsTextPresenter(_playerCharacterParams.BreathPoints, _playerCharacterParamsView.BreathPointsView);
            new PointsTextPresenter(_playerCharacterParams.ArmorPoints, _playerCharacterParamsView.PhysicalArmorPointsView);
            new PointsTextPresenter(_playerCharacterParams.BarrierPoints, _playerCharacterParamsView.MagicShieldPointsView);

            _playerCharacterParams.LevelChanged += OnLevelChanged;
            _playerCharacterParams.PhysicalDamageChanged += OnPhysicalDamageChanged;
            _playerCharacterParams.MagicDamageChanged += OnMagicDamageChanged;
        }

        ~PlayerCharacterParamsPresenter()
        {
            _playerCharacterParams.LevelChanged -= OnLevelChanged;
            _playerCharacterParams.MagicDamageChanged -= OnMagicDamageChanged;
        }

        private void OnLevelChanged(object sender, LevelChangedEventArgs e)
        {
            _playerCharacterParamsView.SetLevelText(e.Level.ToString());
        }

        private void OnMagicDamageChanged(object sender, MagicDamageChangedEventArgs e)
        {
            _playerCharacterParamsView.SetMagicDamageText(e.MagicDamage.ToString());
        }

        private void OnPhysicalDamageChanged(object sender, PhysicalDamageChangedEventArgs e)
        {
            _playerCharacterParamsView.SetPhysicalDamageText(e.PhysicalDamage.ToString());
        }
    }
}
