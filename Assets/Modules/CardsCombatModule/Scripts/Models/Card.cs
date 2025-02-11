using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Managers;

namespace SDRGames.Whist.CardsCombatModule.Models
{
    public class Card : Ability
    {
        public CardModifierScriptableObject[] CardModifiersScriptableObjects { get; private set; }

        public Card(CardScriptableObject cardScriptableObject) : base(cardScriptableObject)
        {
            CardModifiersScriptableObjects = cardScriptableObject.CardModifiersScriptableObjects;
        }

        public void ApplyModifier(int index, CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetCombatManagers, List<Card> affectedCards)
        {
            CardModifiersScriptableObjects[index].Apply(casterCombatManager, targetCombatManagers, affectedCards);
        }

        public void AddEffect(AbilityModifier cardModifier)
        {
            foreach (AbilityLogic abilityLogic in AbilityLogics)
            {
                abilityLogic.AddEffect(cardModifier);
            }
        }
    }
}
