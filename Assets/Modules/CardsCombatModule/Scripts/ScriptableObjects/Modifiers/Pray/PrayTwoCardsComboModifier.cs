using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    public class PrayTwoCardsComboModifier : CardModifierScriptableObject
    {
        [Header("With two cards restores 100% patient's health")]
        [SerializeField] private RestorationLogicScriptableObject _restorationLogicScriptableObject;

        public override void Apply(CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetCombatManagers, List<Card> affectedCards)
        {
            RestorationLogic restorationLogic = new RestorationLogic(_restorationLogicScriptableObject);
            restorationLogic.Apply(casterCombatManager);
        }
    }
}
