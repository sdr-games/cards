using System;
using System.Collections.Generic;
using System.Linq;

using SDRGames.Whist.AnimationsModule;
using SDRGames.Whist.CharacterModule.Models;
using SDRGames.Whist.CharacterModule.Presenters;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Views;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.PointsModule.Models;
using SDRGames.Whist.SoundModule.Controllers;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Managers
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

        public virtual void Initialize(CharacterParamsScriptableObject characterParamsScriptableObject, UserInputController userInputController = null)
        {
            _model = Instantiate(characterParamsScriptableObject.CharacterInfo.Character3DModelData.Model, transform, false);
            AnimationsController.Initialize(_model.GetComponent<Animator>());

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
            PeriodicalEffectView periodicalEffectView = null;
            if (roundsCount > 1)
            {
                periodicalEffectView = Instantiate(_periodicalEffectViewPrefab, GetView().EffectsBar.transform, false);
            }
            _periodicalEffects.Add(new PeriodicalEffectPresenter(roundsCount, changingAction, effectIcon, periodicalEffectView), valuePerRound);
        }

        public void SetBuff(int value, int roundsCount, Sprite effectIcon, string description, Action<int> buffAction)
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
            int debuffBlockChance = UnityEngine.Random.Range(0, 100);
            if(_debuffBlockPercent > debuffBlockChance)
            {
                return;
            }

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
            SoundController.PlayImpact();
            GetView().ShowFloatingText($"{e.Difference}{criticalChar}", GetView().HealthPointsBarView.GetColor());
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
