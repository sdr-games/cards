using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.EnemyBehaviorModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.AIBehaviorModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "SDRGames/Characters/Enemy Data")]
    public class EnemyDataScriptableObject : ScriptableObject
    {
        [field: SerializeField] public CharacterParamsScriptableObject EnemyParamsScriptableObject { get; private set; }
        [field: SerializeField] public SpecialAbilityScriptableObject[] SpecialAbilitiesScriptableObjects { get; private set; }
        [field: SerializeField] public BehaviorScriptableObject[] MeleeBehaviors { get; private set; }
        [field: SerializeField] public BehaviorScriptableObject[] MagicBehaviors { get; private set; }
    }
}
