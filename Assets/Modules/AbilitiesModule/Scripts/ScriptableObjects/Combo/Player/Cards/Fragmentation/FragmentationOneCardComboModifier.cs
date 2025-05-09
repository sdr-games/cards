using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;
using SDRGames.Whist.CharacterCombatModule.Models;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    //[CreateAssetMenu(fileName = "FragmentationOneCardModifier", menuName = "SDRGames/123c")]
    public class FragmentationOneCardComboModifier : AbilityComboScriptableObject
    {
        [Header("With one card returns part (in %) of the damage dealt\nto the patient or player back to the attacker")]
        [SerializeField] private BuffLogicScriptableObject _buffLogicScriptableObject;

        public override void Apply(CharacterCombatManager attackingCombatManager, List<CharacterCombatManager> targetCombatManagers, List<Ability> affectedAbilities)
        {
            BuffLogic buffLogic = new BuffLogic(_buffLogicScriptableObject);
            buffLogic.Apply(attackingCombatManager);
        }

        public override string GetDescription(CharacterParamsModel characterParamsModel)
        {
            BuffLogic buffLogic = new BuffLogic(_buffLogicScriptableObject);
            return $"\n(1): {buffLogic.GetLocalizedDescription(characterParamsModel)}";
        }
    }
}
