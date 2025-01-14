using System;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.CharacterModule.Presenters;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Views;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Managers
{
    public abstract class CharacterCombatManager : MonoBehaviour
    {
        [SerializeField] protected PeriodicalEffectView _periodicalEffectViewPrefab;

        protected Dictionary<int, PeriodicalEffectPresenter> _periodicalEffects;
        protected Dictionary<int, PeriodicalEffectPresenter> _periodicalBuffs;
        protected Dictionary<int, PeriodicalEffectPresenter> _periodicalDebuffs;

        public virtual void Initialize()
        {
            _periodicalEffects = new Dictionary<int, PeriodicalEffectPresenter>();
            _periodicalBuffs = new Dictionary<int, PeriodicalEffectPresenter>();
        }
        public abstract CharacterParamsModel GetParams();
        protected abstract CharacterCombatParamsView GetView();
        public abstract void TakePhysicalDamage(int damage);
        public abstract void TakeMagicalDamage(int damage);
        public abstract void TakeTrueDamage(int damage);
        public abstract void RestoreArmor(int restoration);
        public abstract void RestoreBarrier(int restoration);
        public abstract void RestoreHealth(int restoration);
        public abstract void RestoreStamina(int restoration);
        public abstract void RestoreBreath(int restoration);
        public void SetPeriodicalChanges(int valuePerRound, int roundsCount, Sprite effectIcon, Action<int> changingAction)
        {
            PeriodicalEffectView periodicalEffectView = Instantiate(_periodicalEffectViewPrefab, GetView().EffectsBar.transform, false);
            _periodicalEffects.Add(valuePerRound, new PeriodicalEffectPresenter(roundsCount, changingAction, effectIcon, periodicalEffectView));
        }

        public void SetBuff(int value, int roundsCount, Sprite effectIcon, Action<int> buffAction, bool inPercents = false)
        {
            PeriodicalEffectView periodicalEffectView = Instantiate(_periodicalEffectViewPrefab, GetView().EffectsBar.transform, false);
            _periodicalBuffs.Add(value, new PeriodicalEffectPresenter(roundsCount, buffAction, effectIcon, periodicalEffectView));
            buffAction(value);
        }

        public void SetDebuff(int value, int roundsCount, Sprite effectIcon, Action<int> debuffAction, bool inPercents = false)
        {
            PeriodicalEffectView periodicalEffectView = Instantiate(_periodicalEffectViewPrefab, GetView().EffectsBar.transform, false);
            _periodicalDebuffs.Add(value, new PeriodicalEffectPresenter(roundsCount, debuffAction, effectIcon, periodicalEffectView));
            debuffAction(value);
        }

        public void ApplyPeriodicalEffects()
        {
            Dictionary<int, PeriodicalEffectPresenter> periodicalEffects = new Dictionary<int, PeriodicalEffectPresenter>(_periodicalEffects);
            foreach (var item in periodicalEffects)
            {
                item.Value.ApplyEffect(item.Key);
                item.Value.DecreaseDuration();
                if (item.Value.GetDuration() <= 0)
                {
                    _periodicalEffects.Remove(item.Key);
                    item.Value.Delete();
                }
            }
        }

        public void UpdateBonusesEffects()
        {
            Dictionary<int, PeriodicalEffectPresenter> periodicalBonuses = new Dictionary<int, PeriodicalEffectPresenter>(_periodicalBuffs);
            _periodicalDebuffs.ToList().ForEach(item => periodicalBonuses.Add(item.Key, item.Value));
            foreach (var item in periodicalBonuses)
            {
                item.Value.DecreaseDuration();
                if (item.Value.GetDuration() <= 0)
                {
                    _periodicalBuffs.Remove(item.Key);
                    item.Value.CancelEffect(item.Key);
                    item.Value.Delete();
                }
            }
        }
    }
}
