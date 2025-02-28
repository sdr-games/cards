using System;

using UnityEngine;

namespace SDRGames.Whist.SoundModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "SoundClip", menuName = "SDRGames/Sound/Sound Clip")]
    public class SoundClipScriptableObject : ScriptableObject
    {
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
        [field: SerializeField] public bool Loop { get; private set; }
    }
}
