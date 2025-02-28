using UnityEngine;
using UnityEngine.Pool;

using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.SoundModule.Views;
using System.Collections.Generic;
using System;

namespace SDRGames.Whist.SoundModule.Managers
{
    public class SoundGlobalManager : MonoBehaviour
    {
        public static SoundGlobalManager Instance { get; private set; }

        [SerializeField] private SoundEmitter _soundEmitterPrefab;

        private IObjectPool<SoundEmitter> _soundEmitterPool;
        private List<SoundEmitter> _activeSoundEmitters = new List<SoundEmitter>();

        public void Initialize()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(Instance);

            _soundEmitterPool = new ObjectPool<SoundEmitter>(
                CreateSoundEmitter,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject
            );
        }

        public static SoundEmitter GetSoundEmitter()
        {
            return Instance._soundEmitterPool.Get();
        }

        public static void ReturnToPool(SoundEmitter soundEmitter)
        {
            Instance._soundEmitterPool.Release(soundEmitter);
        }

        private SoundEmitter CreateSoundEmitter()
        {
            SoundEmitter soundEmitter = Instantiate(_soundEmitterPrefab);
            soundEmitter.gameObject.SetActive(false);
            return soundEmitter;
        }

        private void OnTakeFromPool(SoundEmitter soundEmitter)
        {
            soundEmitter.gameObject.SetActive(true);
            _activeSoundEmitters.Add(soundEmitter);
        }

        private void OnReturnedToPool(SoundEmitter soundEmitter)
        {
            soundEmitter.gameObject.SetActive(false);
            _activeSoundEmitters.Remove(soundEmitter);
        }

        private void OnDestroyPoolObject(SoundEmitter soundEmitter)
        {
            if (_activeSoundEmitters.Contains(soundEmitter))
            {
                _activeSoundEmitters.Remove(soundEmitter);
            }
            Destroy(soundEmitter.gameObject);
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_soundEmitterPrefab), _soundEmitterPrefab);
        }
    }
}
