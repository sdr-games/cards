using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    public class PrayOneCardComboModifier : CardModifierScriptableObject
    {
        [Header("With one card restores HP for player and patient in max %")]
        [SerializeField] private RestorationLogicScriptableObject[] _restorationLogicScriptableObjects;

        public override void Apply(CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetCombatManagers, List<Card> affectedCards)
        {
            foreach (RestorationLogicScriptableObject restorationLogicScriptableObject in _restorationLogicScriptableObjects)
            {
                RestorationLogic restorationLogic = new RestorationLogic(restorationLogicScriptableObject);
                restorationLogic.Apply(casterCombatManager);
            }
        }
    }
}
