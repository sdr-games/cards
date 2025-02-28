using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Presenters;
using SDRGames.Whist.CharacterModule.Views;
using SDRGames.Whist.HelpersModule;

using UnityEngine;
using SDRGames.Whist.UserInputModule.Controller;
using System;

namespace SDRGames.Whist.CharacterModule.Managers
{
    public class EnemyCombatManager : CharacterCombatManager
    {
        [SerializeField] private CharacterParamsModel _characterParamsModel;
        [SerializeField] private CharacterCombatParamsView _characterCombatParamsView;
        [SerializeField] private EnemyMeshManager _enemyMeshManager;

        private CharacterCombatParamsPresenter _characterCombatParamsPresenter;

        public event EventHandler<MeshClickedEventArgs> EnemySelected;

        public override void Initialize(UserInputController userInputController)
        {
            base.Initialize();
            _characterCombatParamsPresenter = new CharacterCombatParamsPresenter(_characterParamsModel, _characterCombatParamsView);

            _enemyMeshManager.Initialize(userInputController);
            _enemyMeshManager.MeshClicked += OnMeshClicked;
        }

        public override CharacterParamsModel GetParams()
        {
            return _characterParamsModel;
        }

        protected override CharacterCombatParamsView GetView()
        {
            return _characterCombatParamsView;
        }

        public override void TakePhysicalDamage(int damage)
        {
            _characterCombatParamsPresenter.TakePhysicalDamage(damage);
            base.TakePhysicalDamage(damage);
        }

        public override void TakeMagicalDamage(int damage)
        {
            _characterCombatParamsPresenter.TakeMagicalDamage(damage);
        }

        public override void TakeTrueDamage(int damage)
        {
            _characterCombatParamsPresenter.TakeTrueDamage(damage);
        }

        public override void RestoreArmorPoints(float restoration)
        {
            _characterCombatParamsPresenter.RestoreArmor(restoration);
        }

        public override void RestoreBarrierPoints(float restoration)
        {
            _characterCombatParamsPresenter.RestoreBarrier(restoration);
        }

        public override void RestoreHealthPoints(float restoration)
        {
            _characterCombatParamsPresenter.RestoreHealth(restoration);
        }

        public override void RestoreStaminaPoints(float restoration)
        {
            _characterCombatParamsPresenter.RestoreStamina(restoration);
        }

        public override void RestoreBreathPoints(float restoration)
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

        public bool HasEnoughBreathPoints(float cost)
        {
            if (_characterParamsModel.BreathPoints.CurrentValue < _characterParamsModel.BreathPoints.ReservedValue + cost)
            {
                return false;
            }
            return true;
        }

        protected void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_characterCombatParamsView), _characterCombatParamsView);
            this.CheckFieldValueIsNotNull(nameof(_enemyMeshManager), _enemyMeshManager);
        }

        private void OnMeshClicked(object sender, MeshClickedEventArgs e)
        {
            EnemySelected?.Invoke(this, e);
        }

        private void OnDisable()
        {
            _enemyMeshManager.MeshClicked -= OnMeshClicked;
        }
    }
}
