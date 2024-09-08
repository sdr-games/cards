using System.Collections;
using System.Collections.Generic;
using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesQueueModule
{
    public abstract class AbilityLogicScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string Description { get; protected set; }
        [field: SerializeField] public Sprite EffectIcon { get; protected set; }
        public bool SelfUsable { get; protected set; }

        public abstract void Apply(CharacterCombatManager characterCombatManager);
    }
}
