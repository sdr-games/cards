using System;

using SDRGames.Whist.CharacterCombatModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Presenters;
using SDRGames.Whist.CharacterCombatModule.Views;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.UserInputModule.Controller;
using SDRGames.Whist.CharacterCombatModule.Models;

using UnityEngine;
using SDRGames.Whist.AnimationsModule.Models;

namespace SDRGames.Whist.CharacterCombatModule.Managers
{
    public class EnemyCombatManager : CharacterCombatManager
    {
        [SerializeField] private CharacterCombatUIView _characterCombatParamsViewPrefab;

        private CharacterParamsModel _characterParamsModel;
        private CharacterCombatUIView _characterCombatParamsView;
        private CharacterCombatParamsPresenter _characterCombatParamsPresenter;
        private EnemyMeshManager _enemyMeshManager;

        public event EventHandler<MeshClickedEventArgs> EnemySelected;
        public event EventHandler<BecameInsaneEventArgs> BecameInsane;

        public override void Initialize(CharacterParamsScriptableObject enemyParamsScriptableObject, GameObject modelPrefab, CharacterAnimationsModel animations, UserInputController userInputController)
        {
            _characterParamsModel = new CharacterParamsModel(enemyParamsScriptableObject);
            _characterCombatParamsView = Instantiate(_characterCombatParamsViewPrefab);
            _characterCombatParamsPresenter = new CharacterCombatParamsPresenter(_characterParamsModel, _characterCombatParamsView);

            base.Initialize(enemyParamsScriptableObject, modelPrefab, animations);

            _enemyMeshManager = _model.GetComponent<EnemyMeshManager>();
            _enemyMeshManager.Initialize(userInputController);
            _enemyMeshManager.MeshClicked += OnMeshClicked;
        }

        public override CharacterParamsModel GetParams()
        {
            return _characterParamsModel;
        }

        public override CharacterCombatUIView GetView()
        {
            return _characterCombatParamsView;
        }

        public override void TakePhysicalDamage(int damage, bool isCritical)
        {
            _characterCombatParamsPresenter.TakePhysicalDamage(damage, isCritical);
        }

        public override void TakeMagicalDamage(int damage, bool isCritical)
        {
            _characterCombatParamsPresenter.TakeMagicalDamage(damage, isCritical);
        }

        public override void TakeTrueDamage(int damage, bool isCritical = false)
        {
            _characterCombatParamsPresenter.TakeTrueDamage(damage, isCritical);
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

        public void SpendBreathPoints(float totalCost)
        {
            _characterParamsModel.BreathPoints.DecreaseCurrentValue(totalCost);
        }

        public bool HasEnoughBreathPoints(float cost)
        {
            if (_characterParamsModel.BreathPoints.CurrentValue < _characterParamsModel.BreathPoints.ReservedValue + cost)
            {
                return false;
            }
            return true;
        }

        public void BecomeInsane(int insanityTurns)
        {
            BecameInsane?.Invoke(this, new BecameInsaneEventArgs(insanityTurns));
        }

        private void OnMeshClicked(object sender, MeshClickedEventArgs e)
        {
            EnemySelected?.Invoke(this, e);
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_characterCombatParamsViewPrefab), _characterCombatParamsViewPrefab);
        }

        private void OnDisable()
        {
            _enemyMeshManager.MeshClicked -= OnMeshClicked;
        }
    }
}
