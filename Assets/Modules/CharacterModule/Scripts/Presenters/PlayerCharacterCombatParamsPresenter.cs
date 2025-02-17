using SDRGames.Whist.PointsModule.Presenters;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Views;
using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Presenters
{
    public class PlayerCharacterCombatParamsPresenter : CharacterCombatParamsPresenter
    {
        private PointsBarPresenter _staminaPointsBarPresenter;
        private PointsBarPresenter _breathPointsBarPresenter;

        public PlayerCharacterCombatParamsPresenter(CharacterParamsModel characterParamsModel, PlayerCharacterCombatParamsView playerCharacterCombatParamsView) : base(characterParamsModel, playerCharacterCombatParamsView)
        {
            _staminaPointsBarPresenter = new PointsBarPresenter(characterParamsModel.StaminaPoints, playerCharacterCombatParamsView.StaminaPointsBarView);
            _breathPointsBarPresenter = new PointsBarPresenter(characterParamsModel.BreathPoints, playerCharacterCombatParamsView.BreathPointsBarView);
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
