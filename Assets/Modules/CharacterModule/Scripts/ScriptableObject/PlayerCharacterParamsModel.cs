using System;

using SDRGames.Whist.LocalizationModule.Models;
using SDRGames.Whist.SettingsModule.Models;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "PlayerParameters", menuName = "SDRGames/Characters/Player Parameters")]
    public class PlayerCharacterParamsModel : CharacterParamsModel
    {
        [field: SerializeField] public int Experience { get; private set; } = 0;
        [field: SerializeField] public int TalentPoints { get; private set; } = 0;

        #region Error messages

        [field: Header("Error messages")]
        [field: SerializeField] public LocalizedString NotEnoughtTalentPointsErrorMessage { get; private set; }

        #endregion

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

        public override void ChangeStrength(int strength)
        {
            base.ChangeStrength(strength);
            StrengthChanged?.Invoke(this, new CharactersticChangedEventArgs(Strength));
            PhysicalDamageChanged?.Invoke(this, new ParameterChangedEventArgs(PhysicalDamage));
            PhysicalHitChanceChanged?.Invoke(this, new ParameterChangedEventArgs(PhysicalHitChance));
        }

        public override void ChangeAgility(int agility)
        {
            base.ChangeAgility(agility);
            AgilityChanged?.Invoke(this, new CharactersticChangedEventArgs(Agility));
            StaminaRestorationPowerChanged?.Invoke(this, new ParameterChangedEventArgs(StaminaRestorationPower));
            PiercingChanged?.Invoke(this, new ParameterChangedEventArgs(Piercing));
        }

        public override void ChangeStamina(int stamina)
        {
            base.ChangeStamina(stamina);
            StaminaChanged?.Invoke(this, new CharactersticChangedEventArgs(Stamina));
        }

        public override void ChangeIntelligence(int intelligence)
        {
            base.ChangeIntelligence(intelligence);
            IntelligenceChanged?.Invoke(this, new CharactersticChangedEventArgs(Intelligence));
            MagicalDamageChanged?.Invoke(this, new ParameterChangedEventArgs(MagicalDamage));
            MagicalHitChanceChanged?.Invoke(this, new ParameterChangedEventArgs(MagicalHitChance));
        }

        public void IncreaseTalentPoints(int talentPoints = 1)
        {
            TalentPoints += talentPoints;
            if(TalentPoints < 0)
            {
                TalentPoints = 0;
            } 
            TalentPointsChanged?.Invoke(this, new ParameterChangedEventArgs(TalentPoints));
        }

        public void DecreaseTalentPoints(int talentPoints = 1)
        {
            if(talentPoints > TalentPoints || TalentPoints <= 0)
            {
                return;
            } 
            IncreaseTalentPoints(-talentPoints);
        }

        public void IncreaseExperience(int experience)
        {
            Experience += experience;

            if(Level > Scaling.Instance.ExperienceRequiredPerLevel.Length)
            {
                ExperienceChanged?.Invoke(this, new ExperienceChangedEventArgs(Experience, Experience));
                return;
            }

            if(Experience >= Scaling.Instance.ExperienceRequiredPerLevel[Level - 1])
            {
                IncreaseLevel(1);
                if (Level >= Scaling.Instance.ExperienceRequiredPerLevel.Length)
                {
                    ExperienceChanged?.Invoke(this, new ExperienceChangedEventArgs(Experience, Scaling.Instance.ExperienceRequiredPerLevel[^1]));
                    return;
                }
            } 
            ExperienceChanged?.Invoke(this, new ExperienceChangedEventArgs(Experience, Scaling.Instance.ExperienceRequiredPerLevel[Level - 1]));
        }

        private void OnDisable()
        {
            base.OnDisable();
            Experience = 0;
            TalentPoints = 0;
        }
    }
}
