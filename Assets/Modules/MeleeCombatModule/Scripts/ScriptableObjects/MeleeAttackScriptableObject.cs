using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.MeleeCombatModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MeleeAttackScriptableObject", menuName = "SDRGames/Combat/Melee Attack")]
    public class MeleeAttackScriptableObject : AbilityScriptableObject
    {
        [field: SerializeField] public int Damage { get; private set; }
    }
}
