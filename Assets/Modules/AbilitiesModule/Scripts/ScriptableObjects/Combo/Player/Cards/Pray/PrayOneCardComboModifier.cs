using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;
using SDRGames.Whist.CharacterCombatModule.Models;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    public class PrayOneCardComboModifier : AbilityComboScriptableObject
    {
        [Header("With one card restores HP for player and patient in max %")]
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
            string result = "\n(1): ";
            foreach (RestorationLogicScriptableObject restorationLogicScriptableObject in _restorationLogicScriptableObjects)
            {
                RestorationLogic restorationLogic = new RestorationLogic(restorationLogicScriptableObject);
                result += $"{restorationLogic.GetLocalizedDescription(characterParamsModel)} ";
            }
            return result;
        }
    }
}
