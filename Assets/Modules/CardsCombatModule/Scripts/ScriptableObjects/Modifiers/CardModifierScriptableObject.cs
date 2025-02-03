using System.Collections.Generic;

using SDRGames.Whist.CardsCombatModule.Models;
using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    public abstract class CardModifierScriptableObject : ScriptableObject
    {
        public abstract void Apply(CharacterCombatManager casterCombatManager, List<CharacterCombatManager> targetCombatManager, List<Card> affectedCards);
    }
}
