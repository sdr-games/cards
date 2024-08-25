using SDRGames.Whist.SettingsModule.Views;

using UnityEngine;
using UnityEngine.Audio;

namespace SDRGames.Whist.SettingsModule.Managers
{
    public class AudioSettingsManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;

        public void ChangeGlobalVolume(RangeChangeSettingsEventArgs e)
        {
            _audioMixer.SetFloat("Global", e.Value == -40 ? -80 : e.Value);
        }

        public void ChangeMusicVolume(RangeChangeSettingsEventArgs e)
        {
            _audioMixer.SetFloat("Music", e.Value == -40 ? -80 : e.Value);
        }

        public void ChangeDialoguesVolume(RangeChangeSettingsEventArgs e)
        {
            _audioMixer.SetFloat("Dialogues", e.Value == -40 ? -80 : e.Value);
        }

        public void ChangeAmbientVolume(RangeChangeSettingsEventArgs e)
        {
            _audioMixer.SetFloat("Ambient", e.Value == -40 ? -80 : e.Value);
        }

        public void ChangeSFXVolume(RangeChangeSettingsEventArgs e)
        {
            _audioMixer.SetFloat("SFX", e.Value == -40 ? -80 : e.Value);
        }

        public void ChangeSubtitles(DropdownChangeSettingsEventArgs e)
        {

        }
    }
}
