using SDRGames.Whist.CharacterModule.Presenters;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Views;
using SDRGames.Whist.NotificationsModule;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Managers
{
    public class PlayerCharacterCombatManager : MonoBehaviour
    {
        [SerializeField] private PlayerCharacterParamsModel _playerCharacterParamsModel;
        [SerializeField] private PlayerCharacterCombatParamsView _playerCharacterCombatParamsView;

        private PlayerCharacterCombatParamsPresenter _playerCharacterCombatParamsPresenter;

        private void OnEnable()
        {
            if (_playerCharacterParamsModel == null)
            {
                Debug.LogError("Player Character Params Model не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }

            if (_playerCharacterCombatParamsView == null)
            {
                Debug.LogError("Player Character Combat Params View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }

        public void Initialize()
        {
            _playerCharacterCombatParamsPresenter = new PlayerCharacterCombatParamsPresenter(_playerCharacterParamsModel, _playerCharacterCombatParamsView);
        }

        public bool HasEnoughStaminaPoints(float cost)
        {
            if(_playerCharacterParamsModel.StaminaPoints.CurrentValue < _playerCharacterParamsModel.StaminaPoints.ReservedValue + cost)
            {
                Notification.Show(_playerCharacterCombatParamsPresenter.GetNotEnoughStaminaErrorMessage());
                return false;
            }
            return true;
        }

        public void ReserveStaminaPoints(float cost)
        {
            _playerCharacterCombatParamsPresenter.ReserveStaminaPoints(cost);
        }

        public void SpendStaminaPoints(float totalCost)
        {
            _playerCharacterCombatParamsPresenter.SpendStaminaPoints(totalCost);
        }

        public void RestoreStaminaPoints()
        {
            _playerCharacterCombatParamsPresenter.RestoreStaminaPoints();
        }

        public void ResetStaminaReservedPoints(float reverseAmount)
        {
            _playerCharacterCombatParamsPresenter.ResetStaminaReservedPoints(reverseAmount);
        }

        public bool HasEnoughBreathPoints(float cost)
        {
            if (_playerCharacterParamsModel.BreathPoints.CurrentValue < _playerCharacterParamsModel.BreathPoints.ReservedValue + cost)
            {
                Notification.Show(_playerCharacterCombatParamsPresenter.GetNotEnoughBreathErrorMessage());
                return false;
            }
            return true;
        }

        public void ReserveBreathPoints(float cost)
        {
            _playerCharacterCombatParamsPresenter.ReserveBreathPoints(cost);
        }

        public void SpendBreathPoints(float totalCost)
        {
            _playerCharacterCombatParamsPresenter.SpendBreathPoints(totalCost);
        }

        public void RestoreBreathPoints()
        {
            _playerCharacterCombatParamsPresenter.RestoreBreathPoints();
        }

        public void ResetBreathReservedPoints(float reverseAmount)
        {
            _playerCharacterCombatParamsPresenter.ResetBreathReservedPoints(reverseAmount);
        }
    }
}
