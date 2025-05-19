using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.EnemyBehaviorModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.CharacterInfoModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "SDRGames/Characters/Enemy")]
    public class EnemyScriptableObject : CharacterScriptableObject
    {
        [field: SerializeField] public Vector3 SpawnPosition { get; private set; }
        [field: SerializeField] public Quaternion SpawnRotation { get; private set; }
        [field: SerializeField] public BehaviorScriptableObject[] MeleeBehaviors { get; private set; }
        [field: SerializeField] public BehaviorScriptableObject[] MagicBehaviors { get; private set; }
        [field: SerializeField] public SpecialAbilityScriptableObject[] SpecialAbilitiesScriptableObjects { get; private set; }
    }
}
