using System;

using SDRGames.Whist.CharacterModule.Models;
using SDRGames.Whist.CharacterModule.Views;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Presenters
{
    public class PeriodicalEffectPresenter
    {
        private PeriodicalEffect _periodicalEffect;
        private PeriodicalEffectView _periodicalEffectView;

        public PeriodicalEffectPresenter(int duration, Action<int> action, Sprite effectIcon, PeriodicalEffectView periodicalEffectView)
        {
            _periodicalEffect = new PeriodicalEffect(duration, action);
            _periodicalEffect.DurationChanged += OnDurationChanged;

            _periodicalEffectView = periodicalEffectView;
            _periodicalEffectView.Initialize(effectIcon, duration);
        }

        public void IncreaseDuration(int additionalDuration)
        {
            _periodicalEffect.IncreaseDuration(additionalDuration);
        }

        public void DecreaseDuration(int duration = 1)
        {
            _periodicalEffect.DecreaseDuration(duration);
        }

        public int GetDuration()
        {
            return _periodicalEffect.Duration;
        }

        public void ApplyEffect(int value)
        {
            _periodicalEffect.Action(value);
        }

        public void CancelEffect(int value)
        {
            _periodicalEffect.Action(-value);
        }

        public void Delete()
        {
            _periodicalEffect.DurationChanged -= OnDurationChanged;
            _periodicalEffectView?.Delete();
        }

        private void OnDurationChanged(object sender, DurationChangedEventArgs e)
        {
            _periodicalEffectView?.UpdateDuration(e.Duration);
        }
    }
}
