using System;
using System.Collections;

using SDRGames.Whist.CharacterModule.Presenters;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Views;
using SDRGames.Whist.NotificationsModule;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Managers
{
    public class PlayerCharacterCombatManager : CharacterCombatManager
    {
        [SerializeField] private PlayerCharacterParamsModel _playerCharacterParamsModel;
        [SerializeField] private PlayerCharacterCombatParamsView _playerCharacterCombatParamsView;

        private PlayerCharacterCombatParamsPresenter _playerCharacterCombatParamsPresenter;

        public override void Initialize()
        {
            _playerCharacterCombatParamsPresenter = new PlayerCharacterCombatParamsPresenter(_playerCharacterParamsModel, _playerCharacterCombatParamsView);
        }

        public CharacterParamsModel GetParams()
        {
            return _playerCharacterParamsModel;
        }

        public override void TakePhysicalDamage(int damage)
        {
            _playerCharacterCombatParamsPresenter.TakePhysicalDamage(damage);
        }

        public override void TakeMagicalDamage(int damage)
        {
            _playerCharacterCombatParamsPresenter.TakeMagicalDamage(damage);
        }

        public override void TakeTrueDamage(int damage)
        {
            _playerCharacterCombatParamsPresenter.TakeTrueDamage(damage);
        }

        public override void RestoreArmor(int restoration)
        {
            _playerCharacterCombatParamsPresenter.RestoreArmor(restoration);
        }

        public override void RestoreBarrier(int restoration)
        {
            _playerCharacterCombatParamsPresenter.RestoreBarrier(restoration);
        }

        public override void RestoreHealth(int restoration)
        {
            _playerCharacterCombatParamsPresenter.RestoreHealth(restoration);
        }

        public override void RestoreStamina(int restoration)
        {
            _playerCharacterCombatParamsPresenter.RestoreStamina(restoration);
        }

        public override void RestoreBreath(int restoration)
        {
            _playerCharacterCombatParamsPresenter.RestoreBreath(restoration);
        }

        public override void SetPeriodicalChanges(int valuePerRound, int roundsCount, Sprite effectIcon, Action changingAction)
        {
            if (_periodicalHealthChanges.ContainsKey(valuePerRound))
            {
                _periodicalHealthChanges[valuePerRound].IncreaseDuration(roundsCount);
                return;
            }
            PeriodicalEffectView periodicalEffectView = Instantiate(_periodicalEffectViewPrefab, _playerCharacterCombatParamsView.EffectsBar.transform, false);
            _periodicalHealthChanges.Add(valuePerRound, new PeriodicalEffectPresenter(roundsCount, changingAction, effectIcon, periodicalEffectView));
        }

        public bool HasEnoughStaminaPoints(float cost)
        {
            if(_playerCharacterParamsModel.Stamina.CurrentValue < _playerCharacterParamsModel.Stamina.ReservedValue + cost)
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
            if (_playerCharacterParamsModel.Breath.CurrentValue < _playerCharacterParamsModel.Breath.ReservedValue + cost)
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

        protected override void OnEnable()
        {
            base.OnEnable();
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
    }
}
