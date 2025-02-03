using SDRGames.Whist.AbilitiesModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CardScriptableObject", menuName = "SDRGames/Combat/Cards/Card")]
    public class CardScriptableObject : AbilityScriptableObject
    {
        [field: SerializeField] public CardModifierScriptableObject[] CardModifiersScriptableObjects { get; private set; }
    }
}
