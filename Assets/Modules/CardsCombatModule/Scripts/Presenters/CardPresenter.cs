using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;
using SDRGames.Whist.CardsCombatModule.Views;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Presenters
{
    public class CardPresenter
    {
        public CardPresenter(CardScriptableObject cardScriptableObject, CardView cardView)
        {
            cardView.Initialize(cardScriptableObject.Name, cardScriptableObject.Description, cardScriptableObject.Icon, cardScriptableObject.Cost.ToString());
        }
    }
}
