using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    //[CreateAssetMenu(fileName = "MorphineOneCardComboModifier", menuName = "SDRGames/123c")]
    public class MorphineTwoCardsComboModifier : AbilityComboScriptableObject
    {
        [Header("With two cards decrease damage dealt to the patient in percents and make patient undying")]
        [SerializeField] private BuffLogicScriptableObject[] _buffLogicScriptableObjects;

        public override void Apply(CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetCombatManagers, List<Ability> affectedAbilities)
        {
            foreach (BuffLogicScriptableObject buffLogicScriptableObject in _buffLogicScriptableObjects)
            {
                BuffLogic buffLogic = new BuffLogic(buffLogicScriptableObject);
                buffLogic.Apply(casterCombatManager);
            }
        }

        public override string GetDescription()
        {
            return "";
        }
    }
}
