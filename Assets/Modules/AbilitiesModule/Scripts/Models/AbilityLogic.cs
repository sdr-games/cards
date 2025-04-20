using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.LocalizationModule.Models;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesModule.Models
{
    public abstract class AbilityLogic
    {
        protected LocalizedString _description;
        protected Sprite _effectIcon;
        protected int _targetsCount;
        protected int _chance;
        protected int _roundsCount;


        public bool SelfUsable { get; protected set; }

        public AbilityLogic(AbilityLogicScriptableObject abilityLogicScriptableObject)
        {
            _description = abilityLogicScriptableObject.Description;
            _effectIcon = abilityLogicScriptableObject.EffectIcon;
            _targetsCount = abilityLogicScriptableObject.TargetsCount;
            _chance = abilityLogicScriptableObject.Chance;
            _roundsCount = abilityLogicScriptableObject.RoundsCount + 1;
            SelfUsable = abilityLogicScriptableObject.SelfUsable;
        }

        public abstract void Apply(CharacterCombatManager casterCombatManager);

        public abstract void Apply(CharacterCombatManager casterCombatManager, CharacterCombatManager targetCombatManager);

        public abstract void AddEffect(AbilityModifier cardModifier);

        public abstract string GetLocalizedDescription();
    }
}
