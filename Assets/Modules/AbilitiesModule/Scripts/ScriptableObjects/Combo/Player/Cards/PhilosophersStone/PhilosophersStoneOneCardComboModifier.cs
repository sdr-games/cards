using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    //[CreateAssetMenu(fileName = "PhilosophersStoneOneCardModifier", menuName = "SDRGames/123c")]
    public class PhilosophersStoneOneCardComboModifier : AbilityComboScriptableObject
    {
        [Header("With one card has chance to block 100% damage dealt to the patient")]
        [SerializeField] private BuffLogicScriptableObject _buffLogicScriptableObject;

        public override void Apply(CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetCombatManagers, List<Ability> affectedAbilities)
        {
            BuffLogic buffLogic = new BuffLogic(_buffLogicScriptableObject);
            buffLogic.Apply(casterCombatManager);
        }

        public override string GetDescription()
        {
            return "";
        }
    }
}
