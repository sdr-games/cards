using UnityEngine;

namespace SDRGames.Whist.AIBehaviorModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemiesList", menuName = "SDRGames/Characters/Enemies List")]
    public class EnemiesListScriptableObject : ScriptableObject
    {
        [field: SerializeField] public EnemyDataScriptableObject[] EnemiesData { get; private set; }
    }
}
