using System;
using System.Linq;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;

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

        public int GetScalingMultiplier(AbilityLogicScriptableObject abilityLogicScriptableObject, int currentLevel)
        {
            CardLogicScaling cardLogicScaling = _cardsLogicScalings.FirstOrDefault(item => item.AbilityLogicScriptableObject == abilityLogicScriptableObject);
            return cardLogicScaling.CalculateMultiplier(currentLevel);
        }
    }
}
