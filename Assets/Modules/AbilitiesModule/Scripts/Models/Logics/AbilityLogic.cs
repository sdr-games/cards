using SDRGames.Whist.AbilitiesModule.ScriptableObjects;
using SDRGames.Whist.CharacterCombatModule.Managers;
using SDRGames.Whist.CharacterCombatModule.Models;
using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.PointsModule.Models;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesModule.Models
{
    public abstract class AbilityLogic
    {
        protected int _targetsCount;
        protected int _chance;
        protected int _roundsCount;
        protected bool _inMaxPercents;
        protected bool _inCurrentPercents;
        protected LocalizedString _description;
        protected Sprite _effectIcon;

        public bool SelfUsable { get; protected set; }

        public AbilityLogic(AbilityLogicScriptableObject abilityLogicScriptableObject)
        {
            _targetsCount = abilityLogicScriptableObject.TargetsCount;
            _chance = abilityLogicScriptableObject.Chance;
            _roundsCount = abilityLogicScriptableObject.RoundsCount + 1;
            _inMaxPercents = abilityLogicScriptableObject.InMaxPercents;
            _inCurrentPercents = abilityLogicScriptableObject.InCurrentPercents;
            _description = abilityLogicScriptableObject.Description;
            _effectIcon = abilityLogicScriptableObject.EffectIcon;
            SelfUsable = abilityLogicScriptableObject.SelfUsable;
        }

        public abstract void Apply(CharacterCombatManager casterCombatManager, CharacterCombatManager targetCombatManager = null);

        public abstract void AddEffect(AbilityModifier cardModifier);

        public abstract string GetLocalizedDescription(CharacterParamsModel targetParams);

        protected abstract string GetLocalizedDescription(int value);

        protected abstract int CalculateValue(CharacterParamsModel targetParams);

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
