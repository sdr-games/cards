using SDRGames.Whist.CardsCombatModule.Models;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    //[CreateAssetMenu(fileName = "CardsScaling", menuName = "SDRGames/Combat/Cards/Scaling settings")]
    public class CardsScalingScriptableObject : ScriptableObject
    {
        [SerializeField] private CardsScaling _cardsScalingSettings;

        public void Initialize()
        {
            _cardsScalingSettings.UpdateStaticFields();
        }

        public void OnValidate()
        {
            _cardsScalingSettings.UpdateStaticFields();
        }
    }
}
