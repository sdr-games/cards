using System;
using System.Collections;

using SDRGames.Whist.ActiveBlockModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.ActiveBlockModule.Managers
{
    public class ActiveBlockManager : MonoBehaviour
    {
        [SerializeField] private StanceSwitcherUIView _stanceSwitcherUIView;
        [SerializeField] private ActiveBlockUIView _activeBlockUIView;

        private bool _defensiveStanceActivated;
        private Coroutine _blockingCoroutine;

        public event EventHandler<BlockKeyPressedEventArgs> BlockKeyPressed;
        public event EventHandler<StanceSwitchedEventArgs> StanceSwitched;

        public void Initialize(UserInputController userInputController)
        {
            _stanceSwitcherUIView.Initialize(userInputController);
            _stanceSwitcherUIView.StanceSwitched += OnStanceSwitched;

            _activeBlockUIView.Initialize();
            _activeBlockUIView.BlockKeyPressed += OnBlockKeyPressed;

            _defensiveStanceActivated = false;
        }

        public void StartBlocking(float durationPerChance)
        {
            if (_defensiveStanceActivated)
            {
                _blockingCoroutine = StartCoroutine(StartBlockingCoroutine(durationPerChance));
                return;
            }
            BlockKeyPressed?.Invoke(this, new BlockKeyPressedEventArgs());
        }

        public void StopBlocking()
        {
            if (_blockingCoroutine != null)
            {
                StopCoroutine(_blockingCoroutine);
            }
            _activeBlockUIView.Hide();
        }

        private IEnumerator StartBlockingCoroutine(float duration)
        {
            yield return null;
            _activeBlockUIView.Show();
            _activeBlockUIView.RunIndicator(duration);
        }

        private void OnStanceSwitched(object sender, StanceSwitchedEventArgs e)
        {
            _defensiveStanceActivated = e.DefensiveStanceActive;
            StanceSwitched?.Invoke(this, e);
        }

        private void OnBlockKeyPressed(object sender, BlockKeyPressedEventArgs e)
        {
            StopCoroutine(_blockingCoroutine);
            BlockKeyPressed?.Invoke(sender, e);
        }
    }
}
