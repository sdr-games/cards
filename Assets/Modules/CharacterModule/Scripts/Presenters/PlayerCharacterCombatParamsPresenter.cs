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
            _staminaPointsBarPresenter = new PointsBarPresenter(characterParamsModel.Stamina, playerCharacterCombatParamsView.StaminaPointsBarView);
            _breathPointsBarPresenter = new PointsBarPresenter(characterParamsModel.Breath, playerCharacterCombatParamsView.BreathPointsBarView);
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

        public void RestoreBreathPoints()
        {
            _breathPointsBarPresenter.RestorePoints();
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
