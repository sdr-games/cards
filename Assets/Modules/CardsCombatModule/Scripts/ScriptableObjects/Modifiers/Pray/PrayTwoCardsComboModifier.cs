using System.Collections.Generic;

using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    public class PrayTwoCardsComboModifier : CardModifierScriptableObject
    {
        [Header("Pray in combo with two cards increase damage for every next card by 3%")][SerializeField] private int _valueInPercent = 3;

        public override void Apply(CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetCombatManagers, List<Card> affectedCards)
        {
            foreach (Card card in affectedCards)
            {
                card.AddEffect(new CardModifier(_valueInPercent, true));
            }
        }
    }
}
