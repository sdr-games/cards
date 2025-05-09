using System;

using SDRGames.Whist.CharacterCombatModule.Models;
using SDRGames.Whist.DiceModule.Models;
using SDRGames.Whist.PointsModule.Models;

using UnityEngine;

namespace SDRGames.Whist.CharacterCombatModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "PlayerParameters", menuName = "SDRGames/Characters/Player Parameters")]
    public class PlayerParamsScriptableObject : CharacterParamsScriptableObject
    {
        [field: SerializeField] public Points PatientHealthPoints { get; private set; }
        [field: SerializeField] public int Experience { get; private set; } = 0;
        [field: SerializeField] public int TalentPoints { get; private set; } = 0;

        public event EventHandler<LevelChangedEventArgs> LevelChanged;
        public event EventHandler<ExperienceChangedEventArgs> ExperienceChanged;
        public event EventHandler<CharactersticChangedEventArgs> StrengthChanged;
        public event EventHandler<CharactersticChangedEventArgs> AgilityChanged;
        public event EventHandler<CharactersticChangedEventArgs> StaminaChanged;
        public event EventHandler<CharactersticChangedEventArgs> IntelligenceChanged;
        public event EventHandler<ParameterChangedEventArgs> PhysicalDamageChanged;
        public event EventHandler<ParameterChangedEventArgs> PhysicalHitChanceChanged;
        public event EventHandler<ParameterChangedEventArgs> MagicalDamageChanged;
        public event EventHandler<ParameterChangedEventArgs> MagicalHitChanceChanged;
        public event EventHandler<ParameterChangedEventArgs> StaminaRestorationPowerChanged;
        public event EventHandler<ParameterChangedEventArgs> PiercingChanged;
        public event EventHandler<ParameterChangedEventArgs> TalentPointsChanged;

        public override void IncreaseLevel(int level)
        {
            base.IncreaseLevel(level);
            IncreaseTalentPoints(CharacterParametersScaling.Instance.TalentPointsPerLevel);
            LevelChanged?.Invoke(this, new LevelChangedEventArgs(Level));
            PhysicalDamageChanged?.Invoke(this, new ParameterChangedEventArgs(PhysicalDamageModifier));
            MagicalDamageChanged?.Invoke(this, new ParameterChangedEventArgs(MagicalDamageModifier));
        }

        public override void IncreaseStrength(int strength)
        {
            base.IncreaseStrength(strength);
            StrengthChanged?.Invoke(this, new CharactersticChangedEventArgs(Strength));
            PhysicalDamageChanged?.Invoke(this, new ParameterChangedEventArgs(PhysicalDamageModifier));
            PhysicalHitChanceChanged?.Invoke(this, new ParameterChangedEventArgs(PhysicalHitChance));
        }

        public override void IncreaseAgility(int agility)
        {
            base.IncreaseAgility(agility);
            AgilityChanged?.Invoke(this, new CharactersticChangedEventArgs(Agility));
            StaminaRestorationPowerChanged?.Invoke(this, new ParameterChangedEventArgs(StaminaRestorationPower));
            PiercingChanged?.Invoke(this, new ParameterChangedEventArgs(Piercing));
        }

        public override void IncreaseStamina(int stamina)
        {
            base.IncreaseStamina(stamina);
            StaminaChanged?.Invoke(this, new CharactersticChangedEventArgs(Stamina));
        }

        public override void IncreaseIntelligence(int intelligence)
        {
            base.IncreaseIntelligence(intelligence);
            IntelligenceChanged?.Invoke(this, new CharactersticChangedEventArgs(Intelligence));
            MagicalDamageChanged?.Invoke(this, new ParameterChangedEventArgs(MagicalDamageModifier));
            MagicalHitChanceChanged?.Invoke(this, new ParameterChangedEventArgs(MagicalHitChance));
        }

        public void IncreaseTalentPoints(int talentPoints)
        {
            TalentPoints += talentPoints;
            TalentPointsChanged?.Invoke(this, null);
        }

        public void IncreaseExperience(int experience)
        {
            Experience += experience;

            if(Level > CharacterParametersScaling.Instance.ExperienceRequiredPerLevel.Length)
            {
                ExperienceChanged?.Invoke(this, new ExperienceChangedEventArgs(Experience, Experience));
                return;
            }

            if(Experience >= CharacterParametersScaling.Instance.ExperienceRequiredPerLevel[Level - 1])
            {
                IncreaseLevel(1);
                if (Level >= CharacterParametersScaling.Instance.ExperienceRequiredPerLevel.Length)
                {
                    ExperienceChanged?.Invoke(this, new ExperienceChangedEventArgs(Experience, CharacterParametersScaling.Instance.ExperienceRequiredPerLevel[^1]));
                    return;
                }
            } 
            ExperienceChanged?.Invoke(this, new ExperienceChangedEventArgs(Experience, CharacterParametersScaling.Instance.ExperienceRequiredPerLevel[Level - 1]));
        }        

        protected override void OnEnable()
        {
            base.OnEnable();
            PatientHealthPoints.SetName(nameof(PatientHealthPoints));
            PatientHealthPoints.Reset();
        }

        private void CalculateParameters()
        {
            int physicalDamageLevelScaling = Level / CharacterParametersScaling.Instance.LevelsCountForMultiplier * CharacterParametersScaling.Instance.LevelsToPhysicalDamageMultiplier;
            int magicalDamageLevelScaling = Level / CharacterParametersScaling.Instance.LevelsCountForMultiplier * CharacterParametersScaling.Instance.LevelsToMagicalDamageMultiplier;

            PhysicalDamageModifier = Strength * CharacterParametersScaling.Instance.StrengthToPhysicalDamagePercent;
            PhysicalDamageModifier *= physicalDamageLevelScaling > 0 ? physicalDamageLevelScaling : 1;
            MagicalDamageModifier = Intelligence * CharacterParametersScaling.Instance.IntelligenceToMagicalDamagePercent;
            MagicalDamageModifier *= magicalDamageLevelScaling > 0 ? magicalDamageLevelScaling : 1;
            PhysicalHitChance = Strength * CharacterParametersScaling.Instance.StrengthToPhysicalHitChance;
            MagicalHitChance = Intelligence * CharacterParametersScaling.Instance.IntelligenceToMagicalHitChance;
            Piercing = Agility * CharacterParametersScaling.Instance.AgilityToPiercing;

            DodgeChance = Agility * CharacterParametersScaling.Instance.AgilityToDodgeChance;
            BlockChance = Strength * CharacterParametersScaling.Instance.StrengthToBlockChance;
            OnslaughtChance = Stamina * CharacterParametersScaling.Instance.StaminaToOnslaughtChance;
            CriticalStrikeChance = (Strength + Agility) / 2 * CharacterParametersScaling.Instance.StrengthAndAgilityToCriticalStrikeChance;
            Resilience = Stamina * CharacterParametersScaling.Instance.StaminaToResilienceChance;
            Weakening = 0;
            Amplification = 0;
            Initiative = new Dice("Initiative", 1, 21 - Agility * CharacterParametersScaling.Instance.AgilityToInitiative);

            HealthPoints.SetPermanentBonus(Strength * CharacterParametersScaling.Instance.StrengthToHealthPoints + Stamina * CharacterParametersScaling.Instance.StaminaToHealthPoints);
            StaminaPoints.SetPermanentBonus(Stamina * CharacterParametersScaling.Instance.StaminaToStaminaPoints);
            BreathPoints.SetPermanentBonus(Intelligence * CharacterParametersScaling.Instance.IntelligenceToBreathPoints);
            ArmorPoints.SetPermanentBonus(Level * CharacterParametersScaling.Instance.LevelToArmorPoints);
            BarrierPoints.SetPermanentBonus(Level * CharacterParametersScaling.Instance.LevelToBarrierPoints);

            StaminaPoints.SetRestorationPower(StaminaPoints.MaxValue * (CharacterParametersScaling.Instance.BaseStaminaRestorationPowerPercent / 100) + Agility * CharacterParametersScaling.Instance.AgilityToStaminaRestorationPerRound);
            StunResistance = ArmorPoints.CurrentValue > 0 ? CharacterParametersScaling.Instance.BaseStunResistance : CharacterParametersScaling.Instance.BaseStunResistanceWithoutArmor;
        }

        private void OnValidate()
        {
            if (Strength <= 0)
            {
                Strength = 1;
            }
            if (Agility <= 0)
            {
                Agility = 1;
            }
            if (Stamina <= 0)
            {
                Stamina = 1;
            }
            if (Intelligence <= 0)
            {
                Intelligence = 1;
            }
            CalculateParameters();
        }

        private void OnDisable()
        {
            Level = 1;
            Strength = 1;
            Agility = 1;
            Stamina = 1;
            Intelligence = 1;
            Experience = 0;
            TalentPoints = 0;
            CalculateParameters();
        }
    }
}
