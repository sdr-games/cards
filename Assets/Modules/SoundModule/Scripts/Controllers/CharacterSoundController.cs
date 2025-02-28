using SDRGames.Whist.SoundModule.Managers;
using SDRGames.Whist.SoundModule.ScriptableObjects;
using SDRGames.Whist.SoundModule.Views;

using UnityEngine;

namespace SDRGames.Whist.SoundModule.Controllers
{
    public class CharacterSoundController : MonoBehaviour
    {
        [SerializeField] private SoundClipScriptableObject _impactSoundClip;

        public void Play(SoundClipScriptableObject soundClip)
        {
            SoundEmitter soundEmitter = SoundGlobalManager.GetSoundEmitter();
            soundEmitter.Initialize(soundClip);
            soundEmitter.transform.parent = SoundGlobalManager.Instance.transform;
            soundEmitter.Play();
        }

        public void PlayImpact()
        {
            Play(_impactSoundClip);
        }
    }
}
