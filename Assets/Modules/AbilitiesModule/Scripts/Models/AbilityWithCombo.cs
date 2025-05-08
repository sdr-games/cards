using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;

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

        public override string GetLocalizedDescription()
        {
            string description = base.GetLocalizedDescription();
            foreach (AbilityComboScriptableObject cardModifier in AbilityComboScriptableObjects)
            {
                description += cardModifier.GetDescription();
            }
            return description;
        }
    }
}
