using System;
using System.Collections;

using SDRGames.Whist.ActiveBlockModule.Views;

using UnityEngine;

namespace SDRGames.Whist.ActiveBlockModule.Managers
{
    public class ActiveBlockManager : MonoBehaviour
    {
        //[SerializeField] private ActiveBlockUIView _activeBlockUIView;
        //[SerializeField] private ActiveBlockVariantBUIView _activeBlockVariantBUIView;
        [SerializeField] private ActiveBlockVariantCUIView _activeBlockVariantCUIView;

        private Coroutine _blockingCoroutine;

        public event EventHandler<BlockKeyPressedCEventArgs> BlockKeyPressed;

        public void Initialize()
        {
            //_activeBlockUIView.Initialize();
            //_activeBlockUIView.BlockKeyPressed += OnBlockKeyPressed;
            //_activeBlockVariantBUIView.Initialize();
            //_activeBlockVariantBUIView.BlockKeyPressed += OnBlockKeyPressed;
            _activeBlockVariantCUIView.Initialize();
            _activeBlockVariantCUIView.BlockKeyPressed += OnBlockKeyPressed;
        }

        //public void StartBlocking(int chancesCount, float durationPerChance)
        //{
        //    _blockingCoroutine = StartCoroutine(StartBlockingCoroutine(chancesCount, durationPerChance));
        //}

        //public void StopBlocking()
        //{
        //    if (_blockingCoroutine != null)
        //    {
        //        StopCoroutine(_blockingCoroutine);
        //    } 
        //    _activeBlockUIView.Hide();
        //}

        //private IEnumerator StartBlockingCoroutine(int chancesCount, float durationPerChance)
        //{
        //    yield return null;
        //    _activeBlockUIView.Show();
        //    _activeBlockUIView.StartIndicatorPing(chancesCount, durationPerChance);
        //}

        public void StartBlocking(int chancesCount, float durationPerChance)
        {
            _blockingCoroutine = StartCoroutine(StartBlockingCoroutine(durationPerChance));
        }

        public void StopBlocking()
        {
            if (_blockingCoroutine != null)
            {
                StopCoroutine(_blockingCoroutine);
            }
            //_activeBlockVariantBUIView.Hide();
            _activeBlockVariantCUIView.Hide();
        }

        private IEnumerator StartBlockingCoroutine(float duration)
        {
            yield return null;
            //_activeBlockVariantBUIView.Show();
            //_activeBlockVariantBUIView.RunIndicator(duration);
            _activeBlockVariantCUIView.Show();
            _activeBlockVariantCUIView.RunIndicator(duration);
        }

        //private void OnBlockKeyPressed(object sender, BlockKeyPressedEventArgs e)
        //{
        //    StopCoroutine(_blockingCoroutine);
        //    BlockKeyPressed?.Invoke(sender, e);
        //}

        private void OnBlockKeyPressed(object sender, BlockKeyPressedCEventArgs e)
        {
            StopCoroutine(_blockingCoroutine);
            BlockKeyPressed?.Invoke(sender, e);
        }
    }
}
