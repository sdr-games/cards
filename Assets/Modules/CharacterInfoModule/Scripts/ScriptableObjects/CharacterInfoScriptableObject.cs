using UnityEngine.Localization;

using UnityEngine;

namespace SDRGames.Whist.CharacterInfoModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CharacterInfo", menuName = "SDRGames/Characters/Infos/Character Info")]
    public class CharacterInfoScriptableObject : ScriptableObject
    {
        [field: SerializeField] public bool IsPlayer { get; private set; }
        [field: SerializeField] public LocalizedString CharacterName { get; private set; }
        [field: SerializeField] public LocalizedString CharacterDescription { get; private set; }
        [field: SerializeField] public Sprite CharacterPortrait { get; private set; }
        [field: SerializeField] public Character3DModelScriptableObject Character3DModelData { get; private set; }
    }
}
