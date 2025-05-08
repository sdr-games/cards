using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.CharacterCombatModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesModule.ScriptableObjects
{
    public abstract class AbilityComboScriptableObject : ScriptableObject
    {
        public abstract void Apply(CharacterCombatManager attackingCombatManager, List<CharacterCombatManager> targetCombatManagers, List<Ability> affectedAbilities);

        public abstract string GetDescription();
    }
}
