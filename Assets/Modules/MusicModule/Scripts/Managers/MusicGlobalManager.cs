using SDRGames.Whist.MusicModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.MusicModule.Managers
{
    [RequireComponent(typeof(AudioListener))]
    [RequireComponent(typeof(AudioSource))]
    public class MusicGlobalManager : MonoBehaviour
    {
        public static MusicGlobalManager Instance { get; private set; }

        [SerializeField] private AudioSource _audioSource;

        public static void Play(MusicClipScriptableObject musicClipScriptableObject)
        {
            Instance._audioSource.clip = musicClipScriptableObject.AudioClip;
            Instance._audioSource.loop = musicClipScriptableObject.Loop;
            Instance._audioSource.Play();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }
}
