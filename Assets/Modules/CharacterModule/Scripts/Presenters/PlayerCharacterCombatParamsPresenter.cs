using SDRGames.Whist.PointsModule.Presenters;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Views;

namespace SDRGames.Whist.CharacterModule.Presenters
{
    public class PlayerCharacterCombatParamsPresenter : CommonCharacterCombatParamsPresenter
    {
        private PointsBarPresenter _staminaPointsBarPresenter;

        public PlayerCharacterCombatParamsPresenter(CommonCharacterParamsModel commonCharacterParamsModel, PlayerCharacterCombatParamsView playerCharacterCombatParamsView) : base(commonCharacterParamsModel, playerCharacterCombatParamsView)
        {
            _staminaPointsBarPresenter = new PointsBarPresenter(commonCharacterParamsModel.StaminaPoints, playerCharacterCombatParamsView.StaminaPointsBarView);
            new PointsBarPresenter(commonCharacterParamsModel.BreathPoints, playerCharacterCombatParamsView.BreathPointsBarView);
        }

        public void ReserveStaminaPoints(float cost)
        {
            _staminaPointsBarPresenter.ReservePoints(cost);
        }

        public void SpendStaminaPoints()
        {
            _staminaPointsBarPresenter.SpendPoints();
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
