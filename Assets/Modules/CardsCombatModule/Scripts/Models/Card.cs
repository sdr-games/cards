using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Managers;

namespace SDRGames.Whist.CardsCombatModule.Models
{
    public class Card : Ability
    {
        public CardModifierScriptableObject[] CardModifiersScriptableObjects { get; private set; }

        public Card(CardScriptableObject cardScriptableObject) : base(cardScriptableObject)
        {
            foreach(AbilityLogicScriptableObject abilityLogicScriptableObject in cardScriptableObject.AbilityLogics)
            {
                switch (abilityLogicScriptableObject)
                {
                    case BuffLogicScriptableObject buffLogicScriptableObject:
                        BuffLogic buffLogic = new BuffLogic(buffLogicScriptableObject);
                        AbilityLogics.Add(buffLogic);
                        break;
                    case DamageLogicScriptableObject damageLogicScriptableObject:
                        DamageLogic damageLogic = new DamageLogic(damageLogicScriptableObject);
                        AbilityLogics.Add(damageLogic);
                        break;
                    case RestorationLogicScriptableObject restorationLogicScriptableObject:
                        RestorationLogic restorationLogic = new RestorationLogic(restorationLogicScriptableObject);
                        AbilityLogics.Add(restorationLogic);
                        break;
                    default:
                        break;
                }
            }
            CardModifiersScriptableObjects = cardScriptableObject.CardModifiersScriptableObjects;
        }

        public void ApplyModifier(int index, CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetCombatManagers, List<Card> affectedCards)
        {
            CardModifiersScriptableObjects[index].Apply(casterCombatManager, targetCombatManagers, affectedCards);
        }

        public void AddEffect(CardModifier cardModifier)
        {
            foreach (CardLogic cardLogic in AbilityLogics)
            {
                cardLogic.AddEffect(cardModifier);
            }
        }
    }
}
