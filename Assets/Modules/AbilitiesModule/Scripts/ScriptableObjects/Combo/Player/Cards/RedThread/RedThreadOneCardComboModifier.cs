using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;
using SDRGames.Whist.CharacterCombatModule.Models;

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

        public override string GetDescription(CharacterParamsModel characterParamsModel)
        {
            RestorationLogic restorationLogic = new RestorationLogic(_restorationLogicScriptableObject);
            return $"\n(1): {restorationLogic.GetLocalizedDescription(characterParamsModel)} ";
        }
    }
}
