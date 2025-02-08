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

        protected Dictionary<PeriodicalEffectPresenter, int> _periodicalEffects;
        protected Dictionary<PeriodicalEffectPresenter, int> _periodicalBuffs;
        protected Dictionary<PeriodicalEffectPresenter, int> _periodicalDebuffs;

        public virtual void Initialize()
        {
            _periodicalEffects = new Dictionary<PeriodicalEffectPresenter, int>();
            _periodicalBuffs = new Dictionary<PeriodicalEffectPresenter, int>();
            _periodicalDebuffs = new Dictionary<PeriodicalEffectPresenter, int>();
        }
        public abstract CharacterParamsModel GetParams();
        protected abstract CharacterCombatParamsView GetView();
        public abstract void TakePhysicalDamage(int damage);
        public abstract void TakeMagicalDamage(int damage);
        public abstract void TakeTrueDamage(int damage);
        public abstract void RestoreArmorPoints(float restoration = -1);
        public abstract void RestoreBarrierPoints(float restoration = -1);
        public abstract void RestoreHealthPoints(float restoration = -1);
        public abstract void RestoreStaminaPoints(float restoration = -1);
        public abstract void RestoreBreathPoints(float restoration = -1);

        public void SetPeriodicalChanges(int valuePerRound, int roundsCount, string description, Sprite effectIcon, Action<int> changingAction)
        {
            PeriodicalEffectView periodicalEffectView = null;
            if (roundsCount > 1)
            {
                periodicalEffectView = Instantiate(_periodicalEffectViewPrefab, GetView().EffectsBar.transform, false);
            }
            _periodicalEffects.Add(new PeriodicalEffectPresenter(roundsCount, changingAction, effectIcon, periodicalEffectView), valuePerRound);
        }

        public void SetBuff(int value, int roundsCount, Sprite effectIcon, string description, Action<int> buffAction, bool inPercents = false)
        {
            PeriodicalEffectView periodicalEffectView = null;
            if (roundsCount > 1)
            {
                periodicalEffectView = Instantiate(_periodicalEffectViewPrefab, GetView().EffectsBar.transform, false);
            }
            _periodicalBuffs.Add(new PeriodicalEffectPresenter(roundsCount, buffAction, effectIcon, periodicalEffectView), value);
            buffAction(value);
        }

        public void SetDebuff(int value, int roundsCount, Sprite effectIcon, string description, Action<int> debuffAction, bool inPercents = false)
        {
            PeriodicalEffectView periodicalEffectView = null;
            if(roundsCount > 1)
            {
                periodicalEffectView = Instantiate(_periodicalEffectViewPrefab, GetView().EffectsBar.transform, false);
            } 
            _periodicalDebuffs.Add(new PeriodicalEffectPresenter(roundsCount, debuffAction, effectIcon, periodicalEffectView), value);
            debuffAction(value);
        }

        public void ApplyPeriodicalEffects()
        {
            Dictionary<PeriodicalEffectPresenter, int> periodicalEffects = new Dictionary<PeriodicalEffectPresenter, int>(_periodicalEffects);
            foreach (var item in periodicalEffects)
            {
                item.Key.ApplyEffect(item.Value);
                item.Key.DecreaseDuration();
                if (item.Key.GetDuration() <= 0)
                {
                    _periodicalEffects.Remove(item.Key);
                    item.Key.Delete();
                }
            }
        }

        public void UpdateBonusesEffects()
        {
            Dictionary<PeriodicalEffectPresenter, int> periodicalBonuses = new Dictionary<PeriodicalEffectPresenter, int>(_periodicalBuffs);
            _periodicalDebuffs?.ToList().ForEach(item => periodicalBonuses.Add(item.Key, item.Value));
            foreach (var item in periodicalBonuses)
            {
                item.Key.DecreaseDuration();
                if (item.Key.GetDuration() <= 0)
                {
                    _periodicalBuffs.Remove(item.Key);
                    item.Key.CancelEffect(item.Value);
                    item.Key.Delete();
                }
            }
        }
    }
}
