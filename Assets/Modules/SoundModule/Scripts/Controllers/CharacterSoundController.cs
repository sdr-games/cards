using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.SoundModule.Managers;
using SDRGames.Whist.SoundModule.ScriptableObjects;
using SDRGames.Whist.SoundModule.Views;

using UnityEngine;

namespace SDRGames.Whist.SoundModule.Controllers
{
    public class CharacterSoundController : MonoBehaviour
    {
        [SerializeField] private SoundClipScriptableObject _impactSoundClip;
        [SerializeField] private SoundClipScriptableObject _armorImpactSoundClip;
        [SerializeField] private SoundClipScriptableObject _barrierImpactSoundClip;

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

        public void PlayArmorImpact()
        {
            Play(_armorImpactSoundClip);
        }

        public void PlayBarrierImpact()
        {
            Play(_barrierImpactSoundClip);
        }

        private void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(_impactSoundClip), _impactSoundClip);
            this.CheckFieldValueIsNotNull(nameof(_armorImpactSoundClip), _armorImpactSoundClip);
            this.CheckFieldValueIsNotNull(nameof(_barrierImpactSoundClip), _barrierImpactSoundClip);
        }
    }
}
