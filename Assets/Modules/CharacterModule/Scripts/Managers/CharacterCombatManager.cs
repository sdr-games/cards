using System;
using System.Collections.Generic;

using SDRGames.Whist.CharacterModule.Models;
using SDRGames.Whist.CharacterModule.Presenters;
using SDRGames.Whist.CharacterModule.Views;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Managers
{
    public abstract class CharacterCombatManager : MonoBehaviour
    {
        [SerializeField] protected PeriodicalEffectView _periodicalEffectViewPrefab;

        protected Dictionary<int, PeriodicalEffectPresenter> _periodicalHealthChanges;

        public abstract void Initialize();
        public abstract void TakePhysicalDamage(int damage);
        public abstract void TakeMagicalDamage(int damage);
        public abstract void TakeTrueDamage(int damage);
        public abstract void RestoreArmor(int restoration);
        public abstract void RestoreBarrier(int restoration);
        public abstract void RestoreHealth(int restoration);
        public abstract void RestoreStamina(int restoration);
        public abstract void RestoreBreath(int restoration);
        public abstract void SetPeriodicalChanges(int valuePerRound, int roundsCount, Sprite effectIcon, Action changingAction);

        protected virtual void OnEnable()
        {
            _periodicalHealthChanges = new Dictionary<int, PeriodicalEffectPresenter>();
        }
    }
}
