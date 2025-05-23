using System;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.AnimationsModule;
using SDRGames.Whist.AnimationsModule.Models;
using SDRGames.Whist.CharacterCombatModule.Models;
using SDRGames.Whist.CharacterCombatModule.Presenters;
using SDRGames.Whist.CharacterCombatModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Views;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.PointsModule.Models;
using SDRGames.Whist.SoundModule.Controllers;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.CharacterCombatModule.Managers
{
    public abstract class CharacterCombatManager : MonoBehaviour
    {
        [SerializeField] protected PeriodicalEffectView _periodicalEffectViewPrefab;

        protected Dictionary<PeriodicalEffectPresenter, int> _periodicalEffects;
        protected Dictionary<PeriodicalEffectPresenter, int> _periodicalBuffs;
        protected Dictionary<PeriodicalEffectPresenter, int> _periodicalDebuffs;
        protected GameObject _model;
        protected int _debuffBlockPercent;

        [field: SerializeField] public AnimationsController AnimationsController { get; protected set; }
        [field: SerializeField] public CharacterSoundController SoundController { get; protected set; }

        public virtual void Initialize(CharacterParamsScriptableObject characterParamsScriptableObject, GameObject modelPrefab, CharacterAnimationsModel animations, UserInputController userInputController = null)
        {
            _model = Instantiate(modelPrefab, transform, false);
            AnimationsController.Initialize(_model.GetComponent<Animator>(), animations);

            _periodicalEffects = new Dictionary<PeriodicalEffectPresenter, int>();
            _periodicalBuffs = new Dictionary<PeriodicalEffectPresenter, int>();
            _periodicalDebuffs = new Dictionary<PeriodicalEffectPresenter, int>();
            _debuffBlockPercent = 0;

            GetView().Initialize(transform);

            GetParams().HealthPoints.CurrentValueChanged += OnHealthPointsCurrentValueChanged;
            GetParams().ArmorPoints.CurrentValueChanged += OnArmorPointsCurrentValueChanged;
            GetParams().BarrierPoints.CurrentValueChanged += OnBarrierPointsCurrentValueChanged;
        }

        public abstract CharacterParamsModel GetParams();
        public abstract CharacterCombatUIView GetView();
        public abstract void TakePhysicalDamage(int damage, bool isCritical);
        public abstract void TakeMagicalDamage(int damage, bool isCritical);
        public abstract void TakeTrueDamage(int damage, bool isCritical = false);
        public abstract void RestoreArmorPoints(float restoration = -1);
        public abstract void RestoreBarrierPoints(float restoration = -1);
        public abstract void RestoreHealthPoints(float restoration = -1);
        public abstract void RestoreStaminaPoints(float restoration = -1);
        public abstract void RestoreBreathPoints(float restoration = -1);

        public void SetPeriodicalChanges(int valuePerRound, int roundsCount, string description, Sprite effectIcon, Action<int> changingAction)
        {
            if(GetExistedEffect(changingAction, _periodicalEffects, out PeriodicalEffectPresenter periodicalEffectPresenter))
            {
                periodicalEffectPresenter.IncreaseDuration(roundsCount - 1);
                return;
            }

            PeriodicalEffectView periodicalEffectView = null;
            if (roundsCount > 1)
            {
                periodicalEffectView = Instantiate(_periodicalEffectViewPrefab, GetView().EffectsBar.transform, false);
            }
            _periodicalEffects.Add(new PeriodicalEffectPresenter(roundsCount, changingAction, description, effectIcon, periodicalEffectView), valuePerRound);
        }

        public void SetBuff(int value, int roundsCount, Sprite effectIcon, string description, Action<int> buffAction)
        {
            if (GetExistedEffect(buffAction, _periodicalBuffs, out PeriodicalEffectPresenter periodicalEffectPresenter))
            {
                periodicalEffectPresenter.IncreaseDuration(roundsCount - 1);
                return;
            }

            PeriodicalEffectView periodicalEffectView = null;
            if (roundsCount > 1)
            {
                periodicalEffectView = Instantiate(_periodicalEffectViewPrefab, GetView().EffectsBar.transform, false);
            }
            _periodicalBuffs.Add(new PeriodicalEffectPresenter(roundsCount, buffAction, description, effectIcon, periodicalEffectView), value);
            buffAction(value);
        }

        public void SetDebuff(int value, int roundsCount, Sprite effectIcon, string description, Action<int> debuffAction, bool inPercents = false)
        {
            int debuffBlockChance = UnityEngine.Random.Range(0, 100);
            if(_debuffBlockPercent > debuffBlockChance)
            {
                return;
            }

            if (GetExistedEffect(debuffAction, _periodicalDebuffs, out PeriodicalEffectPresenter periodicalEffectPresenter))
            {
                periodicalEffectPresenter.IncreaseDuration(roundsCount - 1);
                return;
            }

            PeriodicalEffectView periodicalEffectView = null;
            if(roundsCount > 1)
            {
                periodicalEffectView = Instantiate(_periodicalEffectViewPrefab, GetView().EffectsBar.transform, false);
            } 
            _periodicalDebuffs.Add(new PeriodicalEffectPresenter(roundsCount, debuffAction, description, effectIcon, periodicalEffectView), value);
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
            UpdateEffects(_periodicalBuffs);
            UpdateEffects(_periodicalDebuffs);
        }

        public void ClearNegativeEffects()
        {
            ClearEffects(_periodicalEffects);
            ClearEffects(_periodicalDebuffs);
        }

        public void SetDebuffBlock(int percent)
        {
            _debuffBlockPercent = percent;
        }

        private bool GetExistedEffect(Action<int> action, Dictionary<PeriodicalEffectPresenter, int> periodicalEffects, out PeriodicalEffectPresenter periodicalEffectPresenter)
        {
            periodicalEffectPresenter = null;
            if(periodicalEffects.Count == 0)
            {
                return false;
            }
            periodicalEffectPresenter = periodicalEffects.Keys.Where(effect => effect.GetEffect().Method == action.Method).First();
            return periodicalEffectPresenter != null;
        }

        private void UpdateEffects(Dictionary<PeriodicalEffectPresenter, int> effects)
        {
            Dictionary<PeriodicalEffectPresenter, int> _effects = new Dictionary<PeriodicalEffectPresenter, int>(effects);
            foreach (var item in _effects)
            {
                item.Key.DecreaseDuration();
                if (item.Key.GetDuration() <= 0)
                {
                    effects.Remove(item.Key);
                    item.Key.CancelEffect(item.Value);
                    item.Key.Delete();
                }
            }
        }

        private void ClearEffects(Dictionary<PeriodicalEffectPresenter, int> effects)
        {
            Dictionary<PeriodicalEffectPresenter, int> _effects = new Dictionary<PeriodicalEffectPresenter, int>(effects);
            foreach (var item in _effects)
            {
                item.Key.DecreaseDuration(item.Key.GetDuration());
                effects.Remove(item.Key);
                item.Key.CancelEffect(item.Value);
                item.Key.Delete();
            }
        }

        private void OnHealthPointsCurrentValueChanged(object sender, ValueChangedEventArgs e)
        {
            if(e.Difference == 0)
            {
                string missedString = LocalizedString.GetLocalizedString("CombatStatusesAndEffects", "Missed");
                GetView().ShowFloatingText($"<{missedString}>", GetView().HealthPointsBarView.GetColor());
                return;
            }

            string criticalChar = e.IsCritical ? "!" : ""; 
            if(e.Difference > 0)
            {
                GetView().ShowFloatingText($"+{e.Difference}{criticalChar}", GetView().HealthPointsBarView.GetColor());
                return;
            }

            GetView().ShowFloatingText($"{e.Difference}{criticalChar}", GetView().HealthPointsBarView.GetColor());
            if(e.NewValue <= 0)
            {
                AnimationsController.PlayDeathAnimation();
                return;
            }
            AnimationsController.PlayImpactAnimation();
            SoundController.PlayImpact();
        }

        private void OnArmorPointsCurrentValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.Difference == 0)
            {
                string blockedString = LocalizedString.GetLocalizedString("CombatStatusesAndEffects", "Blocked");
                GetView().ShowFloatingText($"<{blockedString}>", GetView().ArmorPointsBarView.GetColor());
                return;
            }

            string criticalChar = e.IsCritical ? "!" : "";
            if (e.Difference > 0)
            {
                GetView().ShowFloatingText($"+{e.Difference}{criticalChar}", GetView().ArmorPointsBarView.GetColor());
                return;
            }
            SoundController.PlayArmorImpact();
            GetView().ShowFloatingText($"{e.Difference}{criticalChar}", GetView().ArmorPointsBarView.GetColor());
        }

        private void OnBarrierPointsCurrentValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.Difference == 0)
            {
                string missedString = LocalizedString.GetLocalizedString("CombatStatusesAndEffects", "Missed");
                GetView().ShowFloatingText($"<{missedString}>", GetView().BarrierPointsBarView.GetColor());
                return;
            }

            string criticalChar = e.IsCritical ? "!" : "";
            if (e.Difference > 0)
            {
                GetView().ShowFloatingText($"+{e.Difference}{criticalChar}", GetView().BarrierPointsBarView.GetColor());
                return;
            }
            GetView().ShowFloatingText($"{e.Difference}{criticalChar}", GetView().BarrierPointsBarView.GetColor());
        }
    }
}
