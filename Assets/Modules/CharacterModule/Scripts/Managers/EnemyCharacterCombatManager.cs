using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Presenters;
using SDRGames.Whist.CharacterModule.Views;

using UnityEngine;
using SDRGames.Whist.HelpersModule;

namespace SDRGames.Whist.CharacterModule.Managers
{
    public class EnemyCharacterCombatManager : CharacterCombatManager
    {
        [SerializeField] private CharacterParamsModel _characterParamsModel;
        [SerializeField] private CharacterCombatParamsView _characterCombatParamsView;

        private CharacterCombatParamsPresenter _characterCombatParamsPresenter;

        public override void Initialize()
        {
            _characterCombatParamsPresenter = new CharacterCombatParamsPresenter(_characterParamsModel, _characterCombatParamsView);
        }

        public CharacterParamsModel GetParams()
        {
            return _characterParamsModel;
        }

        public override void TakePhysicalDamage(int damage)
        {
            _characterCombatParamsPresenter.TakePhysicalDamage(damage);
        }

        public override void TakeMagicalDamage(int damage)
        {
            _characterCombatParamsPresenter.TakeMagicalDamage(damage);
        }

        public override void TakeTrueDamage(int damage)
        {
            _characterCombatParamsPresenter.TakeTrueDamage(damage);
        }

        public override void RestoreArmor(int restoration)
        {
            _characterCombatParamsPresenter.RestoreArmor(restoration);
        }

        public override void RestoreBarrier(int restoration)
        {
            _characterCombatParamsPresenter.RestoreBarrier(restoration);
        }

        public override void RestoreHealth(int restoration)
        {
            _characterCombatParamsPresenter.RestoreHealth(restoration);
        }

        public override void RestoreStamina(int restoration)
        {
            _characterCombatParamsPresenter.RestoreStamina(restoration);
        }

        public override void RestoreBreath(int restoration)
        {
            _characterCombatParamsPresenter.RestoreBreath(restoration);
        }

        public bool HasEnoughStaminaPoints(float cost)
        {
            if (_characterParamsModel.StaminaPoints.CurrentValue < _characterParamsModel.StaminaPoints.ReservedValue + cost)
            {
                return false;
            }
            return true;
        }

        public void SpendStaminaPoints(float totalCost)
        {
            _characterParamsModel.StaminaPoints.DecreaseCurrentValue(totalCost);
        }

        //public void RestoreStaminaPoints()
        //{
        //    _characterCombatParamsPresenter.RestoreStaminaPoints();
        //}

        //public void ResetStaminaReservedPoints(float reverseAmount)
        //{
        //    _characterCombatParamsPresenter.ResetStaminaReservedPoints(reverseAmount);
        //}

        public bool HasEnoughBreathPoints(float cost)
        {
            if (_characterParamsModel.BreathPoints.CurrentValue < _characterParamsModel.BreathPoints.ReservedValue + cost)
            {
                return false;
            }
            return true;
        }

        //public void SpendBreathPoints(float totalCost)
        //{
        //    _characterCombatParamsPresenter.SpendBreathPoints(totalCost);
        //}

        //public void RestoreBreathPoints()
        //{
        //    _characterCombatParamsPresenter.RestoreBreathPoints();
        //}

        //public void ResetBreathReservedPoints(float reverseAmount)
        //{
        //    _characterCombatParamsPresenter.ResetBreathReservedPoints(reverseAmount);
        //}

        public void ApplyPeriodicalEffects()
        {
            foreach(var item in PeriodicalHealthChanges)
            {
                item.Value.Action();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            this.CheckFieldValueIsNotNull(nameof(_characterCombatParamsView), _characterCombatParamsView);
        }
    }
}
