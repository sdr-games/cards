using System;

using SDRGames.Whist.SettingsModule.Models;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "PlayerParameters", menuName = "SDRGames/Characters/Player Parameters")]
    public class PlayerCharacterParamsModel : CharacterParamsModel
    {
        public int Experience { get; private set; }
        public int TalentPoints { get; private set; }

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
            IncreaseTalentPoints(Scaling.Instance.TalentPointsPerLevel);
            LevelChanged?.Invoke(this, new LevelChangedEventArgs(Level));
            PhysicalDamageChanged?.Invoke(this, new ParameterChangedEventArgs(PhysicalDamage));
            MagicalDamageChanged?.Invoke(this, new ParameterChangedEventArgs(MagicalDamage));
        }

        public override void IncreaseStrength(int strength)
        {
            base.IncreaseStrength(strength);
            StrengthChanged?.Invoke(this, new CharactersticChangedEventArgs(Strength));
            PhysicalDamageChanged?.Invoke(this, new ParameterChangedEventArgs(PhysicalDamage));
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
            MagicalDamageChanged?.Invoke(this, new ParameterChangedEventArgs(MagicalDamage));
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
            if(Experience > Scaling.Instance.ExperienceRequiredPerLevel[Level + 2])
            {
                IncreaseLevel(1);
            } 
            ExperienceChanged?.Invoke(this, new ExperienceChangedEventArgs(Experience, Scaling.Instance.ExperienceRequiredPerLevel[Level + 2]));
        }
    }
}
