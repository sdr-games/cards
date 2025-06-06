using System;
using System.Collections.Generic;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesModule.Models
{
    [Serializable]
    public class AbilitySequence
    {
        [field: SerializeField] public AbilityScriptableObject[] Abilities { get; private set; }

        public List<Guid> GetAbilitiesGuids()
        {
            List<Guid> guids = new List<Guid>();
            foreach (AbilityScriptableObject ability in Abilities)
            {
                guids.Add(ability.Guid);
            }
            return guids;
        }
    }
}
