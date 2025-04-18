using SDRGames.Whist.PointsModule.Presenters;
using SDRGames.Whist.CharacterModule.Views;
using SDRGames.Whist.CharacterModule.Models;

namespace SDRGames.Whist.CharacterModule.Presenters
{
    public class PlayerCombatParamsPresenter : CharacterCombatParamsPresenter
    {
        private PointsBarPresenter _staminaPointsBarPresenter;
        private PointsBarPresenter _breathPointsBarPresenter;

        public PlayerCombatParamsPresenter(PlayerParamsModel characterParamsModel, PlayerCharacterCombatUIView playerCharacterCombatParamsView) : base(characterParamsModel, playerCharacterCombatParamsView)
        {
            _staminaPointsBarPresenter = new PointsBarPresenter(characterParamsModel.StaminaPoints, playerCharacterCombatParamsView.StaminaPointsBarView);
            _breathPointsBarPresenter = new PointsBarPresenter(characterParamsModel.BreathPoints, playerCharacterCombatParamsView.BreathPointsBarView);
            new PointsBarPresenter(characterParamsModel.PatientHealthPoints, playerCharacterCombatParamsView.PatientHealthPointsBarView);
        }

        public void TakePatientDamage(int damage)
        {
            ((PlayerParamsModel)_characterParamsModel).TakePatientDamage(damage);
        }

        public void ReserveStaminaPoints(float cost)
        {
            _staminaPointsBarPresenter.ReservePoints(cost);
        }

        public void SpendStaminaPoints(float totalCost)
        {
            _staminaPointsBarPresenter.SpendPoints(totalCost);
        }

        public void ResetStaminaReservedPoints(float reverseAmount)
        {
            _staminaPointsBarPresenter.ResetReservedPoints(reverseAmount);
        }

        public string GetNotEnoughStaminaErrorMessage()
        {
            return _staminaPointsBarPresenter.GetErrorMessage();
        }

        public void ReserveBreathPoints(float cost)
        {
            _breathPointsBarPresenter.ReservePoints(cost);
        }

        public void SpendBreathPoints(float totalCost)
        {
            _breathPointsBarPresenter.SpendPoints(totalCost);
        }

        public void ResetBreathReservedPoints(float reverseAmount)
        {
            _breathPointsBarPresenter.ResetReservedPoints(reverseAmount);
        }

        public string GetNotEnoughBreathErrorMessage()
        {
            return _breathPointsBarPresenter.GetErrorMessage();
        }
    }
}
