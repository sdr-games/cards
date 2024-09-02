using System;

using SDRGames.Whist.TurnSwitchModule.Views;

using UnityEngine;

namespace SDRGames.Whist.TurnSwitchModule.Managers
{
    public class TimerManager : MonoBehaviour
    {
        [SerializeField] private int _combatTurnTime = 30;
        [SerializeField] private int _restorationTurnTime = 120;
        [SerializeField] private TimerView _timerView;

        public event EventHandler TimeOver;

        public void Initialize()
        {
            _timerView.Initialize();
            _timerView.TimeOver += OnTimeOver;
        }

        public void StartCombatTimer()
        {
            _timerView.StartTimer(_combatTurnTime);
        }

        public void StartRestorationTimer()
        {
            _timerView.StartTimer(_restorationTurnTime);
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
