using System;

using SDRGames.Whist.TurnSwitchModule.Views;

using UnityEngine;

namespace SDRGames.Whist.TurnSwitchModule.Managers
{
    public class TimerManager : MonoBehaviour
    {
        [SerializeField] private TimerView _timerView;

        public event EventHandler TimeOver;

        public void Initialize()
        {
            _timerView.Initialize();
            _timerView.TimeOver += OnTimeOver;
        }

        public void StartTimer(int time)
        {
            _timerView.StartTimer(time);
        }

        public void StopTimer()
        {
            _timerView.StopTimer();
        }

        private void OnTimeOver(object sender, EventArgs e)
        {
            TimeOver?.Invoke(sender, e);
        }
    }
}
