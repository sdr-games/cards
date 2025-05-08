using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    public class RedThreadOneCardComboModifier : AbilityComboScriptableObject
    {
        [Header("With one card immidiatelly restores flat value player's health")]
        [SerializeField] private RestorationLogicScriptableObject _restorationLogicScriptableObject;

        public override void Apply(CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetCombatManagers, List<Ability> affectedAbilities)
        {
            RestorationLogic restorationLogic = new RestorationLogic(_restorationLogicScriptableObject);
            restorationLogic.Apply(casterCombatManager);
        }

        public override string GetDescription()
        {
            return "";
        }
    }
}
