using System;

using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.CharacterModule.Presenters;
using SDRGames.Whist.CharacterModule.Views;
using SDRGames.Whist.NotificationsModule;
using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.PointsModule.Models;
using SDRGames.Whist.CharacterModule.Models;

using UnityEngine;
using SDRGames.Whist.CharacterModule.ScriptableObjects;

namespace SDRGames.Whist.CharacterModule.Managers
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        [SerializeField] private PlayerParamsModel _playerParamsModel;
        [SerializeField] private PlayerCharacterCombatUIView _playerCharacterCombatUIView;

        private PlayerCombatParamsPresenter _playerCharacterCombatParamsPresenter;

        public event EventHandler<PatientHealthChangedEventArgs> PatientHealthChanged;

        public override void Initialize(CharacterParamsScriptableObject playerParamsScriptableObject, UserInputController userInputController = null)
        {
            _playerParamsModel = new PlayerParamsModel((PlayerParamsScriptableObject)playerParamsScriptableObject);
            _playerCharacterCombatParamsPresenter = new PlayerCombatParamsPresenter(_playerParamsModel, _playerCharacterCombatUIView);

            _playerParamsModel.StaminaPoints.CurrentValueChanged += OnStaminaPointsCurrentValueChanged;
            _playerParamsModel.BreathPoints.CurrentValueChanged += OnBreathPointsCurrentValueChanged;
            _playerParamsModel.PatientHealthPoints.CurrentValueChanged += OnPatientHealthPointsChanged;

            base.Initialize(playerParamsScriptableObject);
        }

        public override CharacterParamsModel GetParams()
        {
            return _playerParamsModel;
        }

        public override CharacterCombatUIView GetView()
        {
            return _playerCharacterCombatUIView;
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

        public void TakePatientDamage(int damage)
        {
            _playerCharacterCombatParamsPresenter.TakePatientDamage(damage);
        }

        public override void RestoreArmorPoints(float restoration = -1)
        {
            if(restoration < 0)
            {
                restoration = _playerParamsModel.ArmorPoints.RestorationPower;
            } 
            _playerCharacterCombatParamsPresenter.RestoreArmor(restoration);
        }

        public override void RestoreBarrierPoints(float restoration = -1)
        {
            if(restoration < 0)
            {
                restoration = _playerParamsModel.BarrierPoints.RestorationPower;
            } 
            _playerCharacterCombatParamsPresenter.RestoreBarrier(restoration);
        }

        public override void RestoreHealthPoints(float restoration = -1)
        {
            if(restoration < 0)
            {
                restoration = _playerParamsModel.HealthPoints.RestorationPower;
            }
            _playerCharacterCombatParamsPresenter.RestoreHealth(restoration);
        }

        public override void RestoreStaminaPoints(float restoration = -1)
        {
            if(restoration < 0)
            {
                restoration = _playerParamsModel.StaminaPoints.RestorationPower;
            }
            _playerCharacterCombatParamsPresenter.RestoreStamina(restoration);
        }

        public override void RestoreBreathPoints(float restoration = -1)
        {
            if(restoration < 0)
            {
                restoration = _playerParamsModel.BreathPoints.RestorationPower;
            }
            _playerCharacterCombatParamsPresenter.RestoreBreath(restoration);
        }

        public bool HasEnoughStaminaPoints(float cost)
        {
            if(_playerParamsModel.StaminaPoints.CurrentValue < _playerParamsModel.StaminaPoints.ReservedValue + cost)
            {
                NotificationController.Show(_playerCharacterCombatParamsPresenter.GetNotEnoughStaminaErrorMessage());
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

        public void ResetStaminaReservedPoints(float reverseAmount)
        {
            _playerCharacterCombatParamsPresenter.ResetStaminaReservedPoints(reverseAmount);
        }

        public bool HasEnoughBreathPoints(float cost)
        {
            if (_playerParamsModel.BreathPoints.CurrentValue < _playerParamsModel.BreathPoints.ReservedValue + cost)
            {
                NotificationController.Show(_playerCharacterCombatParamsPresenter.GetNotEnoughBreathErrorMessage());
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

        public void ResetBreathReservedPoints(float reverseAmount)
        {
            _playerCharacterCombatParamsPresenter.ResetBreathReservedPoints(reverseAmount);
        }

        protected void OnEnable()
        {
            //this.CheckFieldValueIsNotNull(nameof(_playerParamsModel), _playerParamsModel);
            this.CheckFieldValueIsNotNull(nameof(_playerCharacterCombatUIView), _playerCharacterCombatUIView);
        }

        private void OnStaminaPointsCurrentValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.Difference > 0)
            {
                _playerCharacterCombatUIView.ShowFloatingText($"+{e.Difference}", _playerCharacterCombatUIView.StaminaPointsBarView.GetColor());
            }
        }

        private void OnBreathPointsCurrentValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.Difference > 0)
            {
                _playerCharacterCombatUIView.ShowFloatingText($"+{e.Difference}", _playerCharacterCombatUIView.BreathPointsBarView.GetColor());
            }
        }

        private void OnPatientHealthPointsChanged(object sender, ValueChangedEventArgs e)
        {
            if(e.Difference > 0)
            {
                PatientHealthChanged?.Invoke(this, new PatientHealthChangedEventArgs((int)e.NewValue));
            }
        }
    }
}
