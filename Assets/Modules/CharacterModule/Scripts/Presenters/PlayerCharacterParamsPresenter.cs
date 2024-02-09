using SDRGames.Islands.DiceModule.Presenters;
using SDRGames.Islands.PointsModule.Presenters;
using SDRGames.Whist.CharacterModule.Models;
using SDRGames.Whist.CharacterModule.Views;

using UnityEngine;

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

            _playerCharacterParamsView.Initialize(_playerCharacterParams.CharacterName, _playerCharacterParams.Level.ToString(), _playerCharacterParams.MagicDamageMultiplier.ToString());

            new PointsTextPresenter(_playerCharacterParams.HealthPoints, _playerCharacterParamsView.HealthPointsView);
            new PointsTextPresenter(_playerCharacterParams.StaminaPoints, _playerCharacterParamsView.StaminaPointsView);
            new PointsTextPresenter(_playerCharacterParams.BreathPoints, _playerCharacterParamsView.BreathPointsView);
            new PointsTextPresenter(_playerCharacterParams.Armor, _playerCharacterParamsView.PhysicalArmorPointsView);
            new PointsTextPresenter(_playerCharacterParams.Barrier, _playerCharacterParamsView.MagicShieldPointsView);
            new DicePresenter(_playerCharacterParams.PhysicalDamage, _playerCharacterParamsView.PhysicalDamageDiceView);

            _playerCharacterParams.LevelChanged += OnLevelChanged;
            _playerCharacterParams.MagicDamageMultiplierChanged += OnMagicDamageMultiplierChanged;
        }

        ~PlayerCharacterParamsPresenter()
        {
            Debug.Log("Destructor called");
            _playerCharacterParams.LevelChanged -= OnLevelChanged;
            _playerCharacterParams.MagicDamageMultiplierChanged -= OnMagicDamageMultiplierChanged;
        }

        private void OnLevelChanged(object sender, LevelChangedEventArgs e)
        {
            _playerCharacterParamsView.SetLevelText(e.Level.ToString());
        }

        private void OnMagicDamageMultiplierChanged(object sender, MagicDamageMultiplierChangedEventArgs e)
        {
            _playerCharacterParamsView.SetMagicDamageMultiplierText(e.MagicDamageMultiplier.ToString());
        }
    }
}
