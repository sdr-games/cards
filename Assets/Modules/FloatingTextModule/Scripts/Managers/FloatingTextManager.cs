using System.Collections.Generic;

using SDRGames.Whist.FloatingTextModule.Views;
using SDRGames.Whist.HelpersModule;

using UnityEngine;
using UnityEngine.Pool;

namespace SDRGames.Whist.FloatingTextModule.Managers
{
    public class FloatingTextManager : MonoBehaviour
    {
        public static FloatingTextManager Instance { get; private set; }

        [SerializeField] private FloatingTextView _floatingTextViewPrefab;

        private IObjectPool<FloatingTextView> _floatingTextViewPool;
        private List<FloatingTextView> _activeFloatingTextViews = new List<FloatingTextView>();

        public void Initialize()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;

            _floatingTextViewPool = new ObjectPool<FloatingTextView>(
                CreateFloatingTextView,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject
            );
        }

        public static FloatingTextView GetFloatingTextView()
        {
            return Instance._floatingTextViewPool.Get();
        }

        public static void ReturnToPool(FloatingTextView floatingTextView)
        {
            Instance._floatingTextViewPool.Release(floatingTextView);
        }

        private FloatingTextView CreateFloatingTextView()
        {
            FloatingTextView floatingTextView = Instantiate(_floatingTextViewPrefab);
            floatingTextView.gameObject.SetActive(false);
            return floatingTextView;
        }

        private void OnTakeFromPool(FloatingTextView floatingTextView)
        {
            floatingTextView.gameObject.SetActive(true);
            _activeFloatingTextViews.Add(floatingTextView);
        }

        private void OnReturnedToPool(FloatingTextView floatingTextView)
        {
            floatingTextView.gameObject.SetActive(false);
            _activeFloatingTextViews.Remove(floatingTextView);
        }

        private void OnDestroyPoolObject(FloatingTextView floatingTextView)
        {
            if (_activeFloatingTextViews.Contains(floatingTextView))
            {
                _activeFloatingTextViews.Remove(floatingTextView);
            }
            Destroy(floatingTextView.gameObject);

        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_floatingTextViewPrefab), _floatingTextViewPrefab);
        }
    }
}
