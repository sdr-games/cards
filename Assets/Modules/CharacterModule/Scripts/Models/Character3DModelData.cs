using UnityEngine;

namespace SDRGames.Whist.CharacterModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "3DModelData", menuName = "SDRGames/Characters/3D Model Data")]
    public class Character3DModelData : ScriptableObject
    {
        [field: SerializeField] public GameObject Model { get; private set; }
    }
}
