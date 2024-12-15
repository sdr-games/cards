using System;

using UnityEngine;

namespace SDRGames.Whist.SettingsModule.Models
{
    [Serializable]
    public class Scaling
    {
        public static Scaling Instance { get; private set; }

        #region Strength

        [field: SerializeField] public int StrengthToHealthPoints { get; private set; }
        [field: SerializeField] public int StrengthToPhysicalDamage { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int StrengthToPhysicalHitChance { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int StrengthToBlockChance { get; private set; }

        #endregion

        #region Strength and Agility

        [field: SerializeField][field: Range(0, 100)] public int StrengthAndAgilityToCriticalStrikeChance { get; private set; }

        #endregion

        #region Agility

        [field: SerializeField][field: Range(0, 100)] public int AgilityToDodgeChance { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int AgilityToStaminaRestorationPerRound { get; private set; }
        [field: SerializeField] public int AgilityToInitiative { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int AgilityToPiercing { get; private set; }

        #endregion

        #region Stamina

        [field: SerializeField] public int StaminaToHealthPoints { get; private set; }
        [field: SerializeField] public int StaminaToStaminaPoints { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int StaminaToResilience { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int StaminaToOnslaughtChance { get; private set; }

        #endregion

        #region Intelligence

        [field: SerializeField] public int IntelligenceToBreathPoints { get; private set; }
        [field: SerializeField] public int IntelligenceToMagicalDamage { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int IntelligenceToMagicalHitChance { get; private set; }

        #endregion

        #region Level

        [field: SerializeField] public int[] ExperienceRequiredPerLevel { get; private set; }
        [field: SerializeField] public int TalentPointsPerLevel { get; private set; }
        [field: SerializeField] public int LevelToArmorPoints { get; private set; }
        [field: SerializeField] public int LevelToBarrierPoints { get; private set; }
        [field: SerializeField] public int LevelsCountForMultiplier { get; private set; }
        [field: SerializeField] public int LevelsToPhysicalDamageMultiplier { get; private set; }
        [field: SerializeField] public int LevelsToMagicalDamageMultiplier { get; private set; }

        #endregion

        #region Other

        [field: SerializeField][field: Range(0, 100)] public int WeakeningMaxPercent { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int AmplificationMaxPercent { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public float BaseStaminaRestorationPowerPercent { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int BaseStunResistance { get; private set; }
        [field: SerializeField][field: Range(0, 100)] public int BaseStunResistanceWithoutArmor { get; private set; }
        
        #endregion

        public void UpdateStaticFields()
        {
            if(Instance == null)
            {
                Instance = new Scaling();
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
