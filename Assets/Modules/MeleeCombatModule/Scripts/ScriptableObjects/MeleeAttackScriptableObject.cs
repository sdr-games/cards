using SDRGames.Whist.LocalizationModule.Models;

using UnityEngine;

namespace SDRGames.Whist.MeleeCombatModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MeleeAttackScriptableObject", menuName = "SDRGames/Combat/Melee Attack")]
    public class MeleeAttackScriptableObject : ScriptableObject
    {
        [field: SerializeField] public LocalizedString Name { get; private set; }
        [field: SerializeField] public LocalizedString Description { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int Cost { get; private set; }
    }
}
