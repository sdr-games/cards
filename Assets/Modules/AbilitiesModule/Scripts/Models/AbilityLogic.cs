using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterModule.Managers;
using SDRGames.Whist.CharacterModule.Models;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.PointsModule.Models;

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
        protected bool _inMaxPercents;
        protected bool _inCurrentPercents;

        public bool SelfUsable { get; protected set; }

        public AbilityLogic(AbilityLogicScriptableObject abilityLogicScriptableObject)
        {
            _description = abilityLogicScriptableObject.Description;
            _effectIcon = abilityLogicScriptableObject.EffectIcon;
            _targetsCount = abilityLogicScriptableObject.TargetsCount;
            _chance = abilityLogicScriptableObject.Chance;
            _roundsCount = abilityLogicScriptableObject.RoundsCount + 1;
            _inMaxPercents = abilityLogicScriptableObject.InMaxPercents;
            _inCurrentPercents = abilityLogicScriptableObject.InCurrentPercents;
            SelfUsable = abilityLogicScriptableObject.SelfUsable;
        }

        public abstract void Apply(CharacterCombatManager casterCombatManager, CharacterCombatManager targetCombatManager = null);

        public abstract void AddEffect(AbilityModifier cardModifier);

        public abstract string GetLocalizedDescription();

        protected int CalculatePercentageOfParameter(Points parameter, float percent)
        {
            if(_inCurrentPercents)
            {
                return (int)(percent / 100 * parameter.CurrentValue);
            }
            return (int)(percent / 100 * parameter.MaxValue);
        }

        protected int CalculatePercentageOfParameter(float parameterValue, float percent)
        {
            return (int)(percent / 100 * parameterValue);
        }
    }
}
