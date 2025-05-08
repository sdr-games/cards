using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.MeleeCombatModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.CharacterInfoModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Player", menuName = "SDRGames/Characters/Player")]
    public class PlayerScriptableObject : CharacterScriptableObject
    {
        [field: SerializeField] public MeleeAttackScriptableObject[] MeleeAttacks { get; private set; }
        [field: SerializeField] public DeckScriptableObject[] Decks { get; private set; }
    }
}
