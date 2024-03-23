using UnityEngine.Localization;

using UnityEngine;
using System;

namespace SDRGames.Whist.CharacterModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "CharacterInfo", menuName = "SDRGames/Characters/Character Info")]
    public class CharacterInfoScriptableObject : ScriptableObject
    {
        [field: SerializeField] public Sprite CharacterPortrait { get; private set; }
        [field: SerializeField] public LocalizedString CharacterNameLocalization { get; private set; }
        [field: SerializeField] public LocalizedString CharacterDescriptionLocalization { get; private set; }
    }
}
