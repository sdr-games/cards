using SDRGames.Whist.AnimationsModule.Models;

using UnityEngine;

namespace SDRGames.Whist.CharacterInfoModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "3DModel", menuName = "SDRGames/Characters/Infos/3D Model")]
    public class Character3DModelScriptableObject : ScriptableObject
    {
        [field: SerializeField] public GameObject ModelPrefab { get; private set; }
        [field: SerializeField] public CharacterAnimationsModel Animations { get; private set; }
    }
}
