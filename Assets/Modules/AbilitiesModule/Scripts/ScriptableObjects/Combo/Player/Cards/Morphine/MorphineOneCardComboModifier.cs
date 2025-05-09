using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;
using SDRGames.Whist.CharacterCombatModule.Models;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    //[CreateAssetMenu(fileName = "MorphineOneCardComboModifier", menuName = "SDRGames/123c")]
    public class MorphineOneCardComboModifier : AbilityComboScriptableObject
    {
        [Header("With one card decrease damage dealt to the patient in percents")]
        [SerializeField] private BuffLogicScriptableObject _buffLogicScriptableObject;

        public override void Apply(CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetCombatManagers, List<Ability> affectedAbilities)
        {
            BuffLogic buffLogic = new BuffLogic(_buffLogicScriptableObject);
            buffLogic.Apply(casterCombatManager);
        }

        public override string GetDescription(CharacterParamsModel characterParamsModel)
        {
            BuffLogic buffLogic = new BuffLogic(_buffLogicScriptableObject);
            return $"\n(1): {buffLogic.GetLocalizedDescription(characterParamsModel)}";
        }
    }
}
