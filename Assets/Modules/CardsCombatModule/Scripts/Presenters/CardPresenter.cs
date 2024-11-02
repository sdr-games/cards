using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;
using SDRGames.Whist.CardsCombatModule.Views;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Presenters
{
    public class CardPresenter
    {
        public CardPresenter(Vector3 position, CardScriptableObject cardScriptableObject, CardView cardView)
        {
            cardView.Initialize(position, cardScriptableObject.Name, cardScriptableObject.Description, cardScriptableObject.Icon, cardScriptableObject.Cost.ToString());
        }
    }
}
