using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Presenters;
using SDRGames.Whist.CharacterModule.Views;

using UnityEditor;

using UnityEngine;

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

        public override void TakeMagicalDamage(int damage)
        {
            throw new System.NotImplementedException();
        }

        public override void TakePhysicalDamage(int damage)
        {
            _characterCombatParamsPresenter.TakePhysicalDamage(damage);
        }

        public override void TakeTrueDamage(int damage)
        {
            throw new System.NotImplementedException();
        }

        public override void RestoreArmor(int restoration)
        {
            throw new System.NotImplementedException();
        }

        public override void RestoreBarrier(int restoration)
        {
            throw new System.NotImplementedException();
        }

        public override void RestoreHealth(int restoration)
        {
            throw new System.NotImplementedException();
        }

        public override void RestoreStamina(int restoration)
        {
            throw new System.NotImplementedException();
        }

        public override void RestoreBreath(int restoration)
        {
            throw new System.NotImplementedException();
        }

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
            if (_characterCombatParamsView == null)
            {
                Debug.LogError("Common Character Combat Params View не был назначен");
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
            }
        }
    }
}
