using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.MeleeCombatModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Combination", menuName = "SDRGames/Combat/Melee/Combination")]
    public class MeleeCombinationScriptableObject : AbilityScriptableObject
    {
        [field: SerializeField] public AbilitySequence[] AttackSequence { get; private set; }
        [field: SerializeField] public int EuristicPoints { get; private set; }

        protected override void OnEnable()
        {
            base.OnEnable();
            Cost = 0;
        }

        private void OnValidate()
        {
            Cost = 0;
        }
    }
}
