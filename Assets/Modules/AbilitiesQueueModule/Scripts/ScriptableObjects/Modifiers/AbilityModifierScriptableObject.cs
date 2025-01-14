using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects
{
    public abstract class AbilityModifierScriptableObject : ScriptableObject
    {
        [field: SerializeField][field: ReadOnly] public bool SelfUsable { get; protected set; }
        [field: SerializeField][field: ReadOnly] public bool ComboUsable { get; protected set; }

        public virtual void Apply(CharacterCombatManager targetCharacterCombatManager) { }
        public virtual void Apply(CharacterCombatManager targetCharacterCombatManager, AbilityScriptableObject[] abilities) { }
    }
}
