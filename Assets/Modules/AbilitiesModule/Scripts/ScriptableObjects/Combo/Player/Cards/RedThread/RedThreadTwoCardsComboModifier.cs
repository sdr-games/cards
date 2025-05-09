using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;
using SDRGames.Whist.CharacterCombatModule.Models;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    public class RedThreadTwoCardsComboModifier : AbilityComboScriptableObject
    {
        [Header("With two cards peridoically restores\nflat value player's health, armor and barrier")]
        [SerializeField] private RestorationLogicScriptableObject[] _restorationLogicScriptableObjects;

        public override void Apply(CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetCombatManagers, List<Ability> affectedAbilities)
        {
            foreach (RestorationLogicScriptableObject restorationLogicScriptableObject in _restorationLogicScriptableObjects)
            {
                RestorationLogic restorationLogic = new RestorationLogic(restorationLogicScriptableObject);
                restorationLogic.Apply(casterCombatManager);
            }
        }

        public override string GetDescription(CharacterParamsModel characterParamsModel)
        {
            string result = "\n(2): ";
            foreach (RestorationLogicScriptableObject restorationLogicScriptableObject in _restorationLogicScriptableObjects)
            {
                RestorationLogic restorationLogic = new RestorationLogic(restorationLogicScriptableObject);
                result += $"{restorationLogic.GetLocalizedDescription(characterParamsModel)} ";
            }
            return result;
        }
    }
}
