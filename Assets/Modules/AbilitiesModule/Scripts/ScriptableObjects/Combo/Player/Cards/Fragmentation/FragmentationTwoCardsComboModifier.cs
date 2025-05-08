using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    //[CreateAssetMenu(fileName = "FragmentationTwoCardsModifier", menuName = "SDRGames/123c")]
    public class FragmentationTwoCardsComboModifier : AbilityComboScriptableObject
    {
        [Header("With two cards converts part (in %) of the damage dealt\nto the patient into the lower of the hero's armor and barrier")]
        [SerializeField] private BuffLogicScriptableObject _buffLogicScriptableObject;

        public override void Apply(CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetCombatManagers, List<Ability> affectedAbilities)
        {
            BuffLogic buffLogic = new BuffLogic(_buffLogicScriptableObject);
            buffLogic.Apply(casterCombatManager);    
        }

        public override string GetDescription()
        {
            BuffLogic buffLogic = new BuffLogic(_buffLogicScriptableObject);
            return $"\n(2): {buffLogic.GetLocalizedDescription()}";
        }
    }
}
