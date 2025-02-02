using System;
using System.Linq;

using SDRGames.Whist.CardsCombatModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.Models
{
    [Serializable]
    public class CardsScaling
    {
        public static CardsScaling Instance { get; private set; }

        [SerializeField] private CardLogicScaling[] _cardsLogicScalings;

        public void UpdateStaticFields()
        {
            if (Instance == null)
            {
                Instance = new CardsScaling();
            }

            Instance._cardsLogicScalings = _cardsLogicScalings;
        }

        public int GetScalingMultiplier(CardLogicScriptableObject cardLogicScriptableObject, int currentLevel)
        {
            CardLogicScaling cardLogicScaling = _cardsLogicScalings.FirstOrDefault(item => item.CardLogicScriptableObject == cardLogicScriptableObject);
            return cardLogicScaling.CalculateMultiplier(currentLevel);
        }
    }
}
