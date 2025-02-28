using SDRGames.Whist.SoundModule.ScriptableObjects;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.SoundModule.Managers;

using UnityEngine;

using System.Collections;

namespace SDRGames.Whist.SoundModule.Views
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        private Coroutine _playingCoroutine;

        public void Initialize(SoundClipScriptableObject soundClipScriptableObject)
        {
            _audioSource.clip = soundClipScriptableObject.AudioClip;
            _audioSource.loop = soundClipScriptableObject.Loop;
        }

        public void Play()
        {
            if(_playingCoroutine != null)
            {
                StopCoroutine(_playingCoroutine);
            }
            _audioSource.Play();
            _playingCoroutine = StartCoroutine(WaitForSoundToEnd());
        }

        public void Stop()
        {
            if (_playingCoroutine != null)
            {
                StopCoroutine(_playingCoroutine);
                _playingCoroutine = null;
            }
            _audioSource.Stop();
            SoundGlobalManager.ReturnToPool(this);
        }

        private IEnumerator WaitForSoundToEnd()
        {
            yield return new WaitWhile(() => _audioSource.isPlaying);
            SoundGlobalManager.ReturnToPool(this);
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_audioSource), _audioSource);
        }
    }
}
