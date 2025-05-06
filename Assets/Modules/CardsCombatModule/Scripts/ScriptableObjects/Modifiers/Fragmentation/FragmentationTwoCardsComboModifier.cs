using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    //[CreateAssetMenu(fileName = "FragmentationTwoCardsModifier", menuName = "SDRGames/123c")]
    public class FragmentationTwoCardsComboModifier : CardModifierScriptableObject
    {
        [Header("With two cards converts part (in %) of the damage dealt\nto the patient into the lower of the hero's armor and barrier")]
        [SerializeField] private BuffLogicScriptableObject _buffLogicScriptableObject;

        public override void Apply(CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetCombatManagers, List<Card> affectedCards)
        {
            BuffLogic buffLogic = new BuffLogic(_buffLogicScriptableObject);
            buffLogic.Apply(casterCombatManager);    
        }
    }
}
