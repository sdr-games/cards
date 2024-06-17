using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.CardsCombatModule.Presenters;
using SDRGames.Whist.CardsCombatModule.ScriptableObjects;
using SDRGames.Whist.CardsCombatModule.Views;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Managers
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField] private CardView _cardView;

        public void Initialize(Vector3 position, CardScriptableObject cardScriptableObject)
        {
            new CardPresenter(position, cardScriptableObject, _cardView);
        }
    }
}
