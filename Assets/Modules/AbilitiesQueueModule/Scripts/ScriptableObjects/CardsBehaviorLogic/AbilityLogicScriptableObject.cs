using System.Collections;
using System.Collections.Generic;
using SDRGames.Whist.CharacterModule.Managers;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects
{
    public abstract class AbilityLogicScriptableObject : ScriptableObject
    {
        [field: SerializeField] public string Description { get; protected set; }
        [field: SerializeField] public Sprite EffectIcon { get; protected set; }
        [field: SerializeField] public int TargetsCount { get; protected set; } = 1;
        [SerializeField][Range(0, 100)] protected int _chance = 100;
        public bool SelfUsable { get; protected set; }

        public abstract void Apply(CharacterCombatManager characterCombatManager);
    }
}
