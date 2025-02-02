using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.CardsCombatModule.ScriptableObjects;

namespace SDRGames.Whist.CardsCombatModule
{
    public abstract class CardLogic : AbilityLogic
    {
        public CardLogic(CardLogicScriptableObject cardLogicScriptableObject) : base(cardLogicScriptableObject)
        {

        }

        public abstract void AddEffect(CardModifier cardModifier);
    }
}
