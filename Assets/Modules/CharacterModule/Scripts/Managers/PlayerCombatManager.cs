using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.CharacterModule.Presenters;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Views;
using SDRGames.Whist.NotificationsModule;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;
using SDRGames.Whist.PointsModule.Models;

namespace SDRGames.Whist.CharacterModule.Managers
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        [SerializeField] private PlayerCharacterParamsModel _playerCharacterParamsModel;
        [SerializeField] private PlayerCharacterCombatUIView _playerCharacterCombatUIView;

        private PlayerCharacterCombatParamsPresenter _playerCharacterCombatParamsPresenter;

        public override void Initialize(UserInputController userInputController = null)
        {
            base.Initialize();
            _playerCharacterCombatParamsPresenter = new PlayerCharacterCombatParamsPresenter(_playerCharacterParamsModel, _playerCharacterCombatUIView);

            _playerCharacterParamsModel.StaminaPoints.CurrentValueChanged += OnStaminaPointsCurrentValueChanged;
            _playerCharacterParamsModel.BreathPoints.CurrentValueChanged += OnBreathPointsCurrentValueChanged;
        }

        public override CharacterParamsModel GetParams()
        {
            return _playerCharacterParamsModel;
        }

        protected override CharacterCombatUIView GetView()
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

        public override void RestoreArmorPoints(float restoration = -1)
        {
            if(restoration < 0)
            {
                restoration = _playerCharacterParamsModel.ArmorPoints.RestorationPower;
            } 
            _playerCharacterCombatParamsPresenter.RestoreArmor(restoration);
        }

        public override void RestoreBarrierPoints(float restoration = -1)
        {
            if(restoration < 0)
            {
                restoration = _playerCharacterParamsModel.BarrierPoints.RestorationPower;
            } 
            _playerCharacterCombatParamsPresenter.RestoreBarrier(restoration);
        }

        public override void RestoreHealthPoints(float restoration = -1)
        {
            if(restoration < 0)
            {
                restoration = _playerCharacterParamsModel.HealthPoints.RestorationPower;
            }
            _playerCharacterCombatParamsPresenter.RestoreHealth(restoration);
        }

        public override void RestoreStaminaPoints(float restoration = -1)
        {
            if(restoration < 0)
            {
                restoration = _playerCharacterParamsModel.StaminaPoints.RestorationPower;
            }
            _playerCharacterCombatParamsPresenter.RestoreStamina(restoration);
        }

        public override void RestoreBreathPoints(float restoration = -1)
        {
            if(restoration < 0)
            {
                restoration = _playerCharacterParamsModel.BreathPoints.RestorationPower;
            }
            _playerCharacterCombatParamsPresenter.RestoreBreath(restoration);
        }

        public bool HasEnoughStaminaPoints(float cost)
        {
            if(_playerCharacterParamsModel.StaminaPoints.CurrentValue < _playerCharacterParamsModel.StaminaPoints.ReservedValue + cost)
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
            if (_playerCharacterParamsModel.BreathPoints.CurrentValue < _playerCharacterParamsModel.BreathPoints.ReservedValue + cost)
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
            this.CheckFieldValueIsNotNull(nameof(_playerCharacterParamsModel), _playerCharacterParamsModel);
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
    }
}
