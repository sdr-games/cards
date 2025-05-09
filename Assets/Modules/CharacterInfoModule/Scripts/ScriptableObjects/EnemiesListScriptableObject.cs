using UnityEngine;

namespace SDRGames.Whist.CharacterInfoModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemiesList", menuName = "SDRGames/Characters/Enemies List")]
    public class EnemiesListScriptableObject : ScriptableObject
    {
        [field: SerializeField] public EnemyScriptableObject[] EnemiesData { get; private set; }
    }
}
