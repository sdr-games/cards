using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;
using SDRGames.Whist.CharacterCombatModule.Models;

namespace SDRGames.Whist.AbilitiesModule.Models
{
    public class AbilityWithCombo : Ability
    {
        public AbilityComboScriptableObject[] AbilityComboScriptableObjects { get; private set; }

        public AbilityWithCombo(AbilityWithComboScriptableObject abilityScriptableObject) : base(abilityScriptableObject)
        {
            AbilityComboScriptableObjects = abilityScriptableObject.AbilityComboScriptableObjects;
        }

        public void ApplyCombo(CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetCombatManagers, List<Ability> affectedCards)
        {
            int index = affectedCards.Count - 1;
            AbilityComboScriptableObjects[index].Apply(casterCombatManager, targetCombatManagers, affectedCards);
        }

        public void AddEffect(AbilityModifier cardModifier)
        {
            foreach (AbilityLogic abilityLogic in AbilityLogics)
            {
                abilityLogic.AddEffect(cardModifier);
            }
        }

        public override string GetLocalizedDescription(CharacterParamsModel characterParamsModel)
        {
            string description = base.GetLocalizedDescription(characterParamsModel);
            foreach (AbilityComboScriptableObject abilityComboScriptableObject in AbilityComboScriptableObjects)
            {
                description += abilityComboScriptableObject.GetDescription(characterParamsModel);
            }
            return description;
        }
    }
}
