using SDRGames.Whist.PointsModule.Presenters;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Views;

namespace SDRGames.Whist.CharacterModule.Presenters
{
    public class PlayerCharacterCombatParamsPresenter : CharacterCombatParamsPresenter
    {
        private PointsBarPresenter _staminaPointsBarPresenter;

        public PlayerCharacterCombatParamsPresenter(CharacterParamsModel characterParamsModel, PlayerCharacterCombatParamsView playerCharacterCombatParamsView) : base(characterParamsModel, playerCharacterCombatParamsView)
        {
            _staminaPointsBarPresenter = new PointsBarPresenter(characterParamsModel.StaminaPoints, playerCharacterCombatParamsView.StaminaPointsBarView);
            new PointsBarPresenter(characterParamsModel.BreathPoints, playerCharacterCombatParamsView.BreathPointsBarView);
        }

        public void ReserveStaminaPoints(float cost)
        {
            _staminaPointsBarPresenter.ReservePoints(cost);
        }

        public void SpendStaminaPoints(float totalCost)
        {
            _staminaPointsBarPresenter.SpendPoints(totalCost);
        }

        public void RestoreStaminaPoints()
        {
            _staminaPointsBarPresenter.RestorePoints();
        }

        public void ResetReservedPoints(float reverseAmount)
        {
            _staminaPointsBarPresenter.ResetReservedPoints(reverseAmount);
        }

        public string GetNotEnoughStaminaErrorMessage()
        {
            return _staminaPointsBarPresenter.GetErrorMessage();
        }
    }
}
