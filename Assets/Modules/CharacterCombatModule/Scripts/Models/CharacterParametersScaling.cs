using System;

using UnityEngine;

namespace SDRGames.Whist.CharacterCombatModule.Models
{
    [Serializable]
    public class CharacterParametersScaling
    {
        public static CharacterParametersScaling Instance { get; private set; }

        #region Strength

        [field: Header("STRENGTH")][field: SerializeField] public int StrengthToHealthPoints { get; private set; }
        [field: SerializeField] public int StrengthToPhysicalDamage { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int StrengthToPhysicalHitChance { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int StrengthToBlockChance { get; private set; }

        #endregion

        #region Strength and Agility

        [field: Header("STRENGTH AND AGILITY")][field: SerializeField][field: Range(0, 100)] public int StrengthAndAgilityToCriticalStrikeChance { get; private set; }

        #endregion

        #region Agility

        [field: Header("AGILITY")][field: SerializeField][field: Range(0, 100)] public int AgilityToDodgeChance { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int AgilityToStaminaRestorationPerRound { get; private set; }
        [field: SerializeField] public int AgilityToInitiative { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int AgilityToPiercing { get; private set; }

        #endregion

        #region Stamina

        [field: Header("STAMINA")][field: SerializeField] public int StaminaToHealthPoints { get; private set; }
        [field: SerializeField] public int StaminaToStaminaPoints { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int StaminaToResilience { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int StaminaToOnslaughtChance { get; private set; }

        #endregion

        #region Intelligence

        [field: Header("INTELLIGENCE")][field: SerializeField] public int IntelligenceToBreathPoints { get; private set; }
        [field: SerializeField] public int IntelligenceToMagicalDamage { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int IntelligenceToMagicalHitChance { get; private set; }

        #endregion

        #region Level

        [field: Header("LEVEL")][field: SerializeField] public int[] ExperienceRequiredPerLevel { get; private set; }
        [field: SerializeField] public int TalentPointsPerLevel { get; private set; }
        [field: SerializeField] public int LevelToArmorPoints { get; private set; }
        [field: SerializeField] public int LevelToBarrierPoints { get; private set; }
        [field: SerializeField] public int LevelsCountForMultiplier { get; private set; }
        [field: SerializeField] public int LevelsToPhysicalDamageMultiplier { get; private set; }
        [field: SerializeField] public int LevelsToMagicalDamageMultiplier { get; private set; }

        #endregion

        #region Other

        [field: Header("OTHER")][field: SerializeField][field: Range(0, 100)] public int WeakeningMaxPercent { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int AmplificationMaxPercent { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int ResiliencePercent { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public float BaseStaminaRestorationPowerPercent { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int BaseStunResistance { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int BaseStunResistanceWithoutArmor { get; private set; }
        [field: SerializeField] public float CriticalStrikeModifier { get; private set; } = 1.5f;
        
        #endregion

        public void UpdateStaticFields()
        {
            if(Instance == null)
            {
                Instance = new CharacterParametersScaling();
            }

            Instance.StrengthToHealthPoints = StrengthToHealthPoints;
            Instance.StrengthToPhysicalDamage = StrengthToPhysicalDamage;
            Instance.StrengthToPhysicalHitChance = StrengthToPhysicalHitChance;
            Instance.StrengthToBlockChance = StrengthToBlockChance;

            Instance.StrengthAndAgilityToCriticalStrikeChance = StrengthAndAgilityToCriticalStrikeChance;

            Instance.AgilityToDodgeChance = AgilityToDodgeChance;
            Instance.AgilityToStaminaRestorationPerRound = AgilityToStaminaRestorationPerRound;
            Instance.AgilityToInitiative = AgilityToInitiative;
            Instance.AgilityToPiercing = AgilityToPiercing;

            Instance.StaminaToHealthPoints = StaminaToHealthPoints;
            Instance.StaminaToStaminaPoints = StaminaToStaminaPoints;
            Instance.StaminaToResilience = StaminaToResilience;
            Instance.StaminaToOnslaughtChance = StaminaToOnslaughtChance;

            Instance.IntelligenceToBreathPoints = IntelligenceToBreathPoints;
            Instance.IntelligenceToMagicalDamage = IntelligenceToMagicalDamage;
            Instance.IntelligenceToMagicalHitChance = IntelligenceToMagicalHitChance;

            Instance.ExperienceRequiredPerLevel = ExperienceRequiredPerLevel;
            Instance.TalentPointsPerLevel = TalentPointsPerLevel;
            Instance.LevelToArmorPoints = LevelToArmorPoints;
            Instance.LevelToBarrierPoints = LevelToBarrierPoints;
            Instance.LevelsCountForMultiplier = LevelsCountForMultiplier;
            Instance.LevelsToPhysicalDamageMultiplier = LevelsToPhysicalDamageMultiplier;
            Instance.LevelsToMagicalDamageMultiplier = LevelsToMagicalDamageMultiplier;

            Instance.WeakeningMaxPercent = WeakeningMaxPercent;
            Instance.AmplificationMaxPercent = AmplificationMaxPercent;
            Instance.BaseStaminaRestorationPowerPercent = BaseStaminaRestorationPowerPercent;
            Instance.BaseStunResistance = BaseStunResistance;
            Instance.BaseStunResistanceWithoutArmor = BaseStunResistanceWithoutArmor;
        }
    }
}
