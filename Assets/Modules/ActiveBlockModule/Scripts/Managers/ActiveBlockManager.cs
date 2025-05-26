using System;
using System.Collections;

using SDRGames.Whist.ActiveBlockModule.Views;
using SDRGames.Whist.UserInputModule.Controller;

using UnityEngine;

namespace SDRGames.Whist.ActiveBlockModule.Managers
{
    public class ActiveBlockManager : MonoBehaviour
    {
        [SerializeField] private ActiveBlockUIView _activeBlockUIView;

        private Coroutine _blockingCoroutine;

        public event EventHandler<BlockKeyPressedEventArgs> BlockKeyPressed;

        public void Initialize()
        {
            _activeBlockUIView.Initialize();
            _activeBlockUIView.BlockKeyPressed += OnBlockKeyPressed;
        }

        public void StartBlocking(int chancesCount, float durationPerChance)
        {
            _blockingCoroutine = StartCoroutine(StartBlockingCoroutine(chancesCount, durationPerChance));
        }

        public void StopBlocking()
        {
            if (_blockingCoroutine != null)
            {
                StopCoroutine(_blockingCoroutine);
            } 
            _activeBlockUIView.Hide();
        }

        private IEnumerator StartBlockingCoroutine(int chancesCount, float durationPerChance)
        {
            yield return null;
            _activeBlockUIView.Show();
            _activeBlockUIView.StartIndicatorPing(chancesCount, durationPerChance);
        }

        private void OnBlockKeyPressed(object sender, BlockKeyPressedEventArgs e)
        {
            StopCoroutine(_blockingCoroutine);
            BlockKeyPressed?.Invoke(sender, e);
        }
    }
}
