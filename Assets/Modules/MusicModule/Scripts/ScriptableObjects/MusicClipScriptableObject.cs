using System;

using UnityEngine;

namespace SDRGames.Whist.MusicModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "MusicClip", menuName = "SDRGames/Music/Music Clip")]
    public class MusicClipScriptableObject : ScriptableObject
    {
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
        [field: SerializeField] public bool Loop { get; private set; }
    }
}
