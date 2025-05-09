using SDRGames.Whist.CharacterCombatModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.CharacterInfoModule.ScriptableObjects
{
    public class CharacterScriptableObject : ScriptableObject
    {
        [field: SerializeField] public CharacterInfoScriptableObject CharacterInfo { get; private set; }
        [field: SerializeField] public CharacterParamsScriptableObject CharacterParams { get; private set; }
    }
}
