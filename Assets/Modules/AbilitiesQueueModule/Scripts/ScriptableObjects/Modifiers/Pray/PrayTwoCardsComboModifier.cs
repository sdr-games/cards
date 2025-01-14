using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesQueueModule
{
    public class PrayTwoCardsComboModifier : AbilityModifierScriptableObject
    {
        [SerializeField] private int _damageIncreasePercent = 3;

        public override void Apply(CharacterCombatManager characterCombatManager)
        {
            throw new System.NotImplementedException();
        }
    }
}
