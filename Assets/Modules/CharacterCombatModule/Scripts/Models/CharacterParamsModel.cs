using SDRGames.Whist.CharacterCombatModule.ScriptableObjects;
using SDRGames.Whist.PointsModule.Models;

namespace SDRGames.Whist.CharacterCombatModule.Models
{
    public class CharacterParamsModel
    {
        public int Level { get; protected set; }
        public int Strength { get; protected set; }
        public int Agility { get; protected set; }
        public int Stamina { get; protected set; }
        public int Intelligence { get; protected set; }
        public int PhysicalDamageModifier { get; protected set; }
        public float TotalPhysicalDamagePercent { get; protected set; }
        public int MagicalDamageModifier { get; protected set; }
        public float TotalMagicalDamagePercent { get; protected set; }
        public int PhysicalHitChance { get; protected set; }
        public int MagicalHitChance { get; protected set; }
        public float StaminaRestorationPower { get => StaminaPoints.RestorationPower; }
        public int Piercing { get; protected set; }
        public int DodgeChance { get; protected set; }
        public int BlockChance { get; protected set; }
        public int OnslaughtChance { get; protected set; }
        public int CriticalStrikeChance { get; protected set; }
        public int ResilienceChance { get; protected set; }
        public int Weakening { get; protected set; }
        public int Amplification { get; protected set; }
        public int StunResistance { get; protected set; }
        public Points HealthPoints { get; protected set; }
        public Points StaminaPoints { get; protected set; }
        public Points BreathPoints { get; protected set; }
        public Points ArmorPoints { get; protected set; }
        public Points BarrierPoints { get; protected set; }
        public int PhysicalDamageBlockPercent { get; protected set; }
        public int MagicalDamageBlockPercent { get; protected set; }
        public int ThornsPercent { get; protected set; }
        public int DebuffBlockPercent { get; protected set; }
        public int AdvantagePercent { get; protected set; }

        public CharacterParamsModel(CharacterParamsScriptableObject characterParamsScriptableObject)
        {
            Level = characterParamsScriptableObject.Level;
            Strength = characterParamsScriptableObject.Strength;
            Agility = characterParamsScriptableObject.Agility;
            Stamina = characterParamsScriptableObject.Stamina;
            Intelligence = characterParamsScriptableObject.Intelligence;

            PhysicalDamageModifier = characterParamsScriptableObject.PhysicalDamageModifier;
            TotalPhysicalDamagePercent = 1;
            MagicalDamageModifier = characterParamsScriptableObject.MagicalDamageModifier;
            TotalMagicalDamagePercent = 1;
            PhysicalHitChance = characterParamsScriptableObject.PhysicalHitChance;
            MagicalHitChance = characterParamsScriptableObject.MagicalHitChance;
            Piercing = characterParamsScriptableObject.Piercing;
            DodgeChance = characterParamsScriptableObject.DodgeChance;
            BlockChance = characterParamsScriptableObject.BlockChance;
            OnslaughtChance = characterParamsScriptableObject.OnslaughtChance;
            CriticalStrikeChance = characterParamsScriptableObject.CriticalStrikeChance;
            ResilienceChance = characterParamsScriptableObject.Resilience;
            Weakening = characterParamsScriptableObject.Weakening;
            Amplification = characterParamsScriptableObject.Amplification;
            StunResistance = characterParamsScriptableObject.StunResistance;

            HealthPoints = new Points(characterParamsScriptableObject.HealthPoints);
            StaminaPoints = new Points(characterParamsScriptableObject.StaminaPoints);
            BreathPoints = new Points(characterParamsScriptableObject.BreathPoints);
            ArmorPoints = new Points(characterParamsScriptableObject.ArmorPoints);
            BarrierPoints = new Points(characterParamsScriptableObject.BarrierPoints);
        }

        public virtual void ChangeStrength(int strength)
        {
            Strength += strength;
            int physicalDamageLevelScaling = Level / CharacterParametersScaling.Instance.LevelsCountForMultiplier * CharacterParametersScaling.Instance.LevelsToPhysicalDamageMultiplier;

            PhysicalDamageModifier = Strength * CharacterParametersScaling.Instance.StrengthToPhysicalDamagePercent;
            PhysicalDamageModifier *= physicalDamageLevelScaling > 0 ? physicalDamageLevelScaling : 1;
            PhysicalHitChance = Strength * CharacterParametersScaling.Instance.StrengthToPhysicalHitChance + AdvantagePercent;
            BlockChance = Strength * CharacterParametersScaling.Instance.StrengthToBlockChance;
            CriticalStrikeChance = (Strength + Agility) / 2 * CharacterParametersScaling.Instance.StrengthAndAgilityToCriticalStrikeChance;
            HealthPoints.SetPermanentBonus(Strength * CharacterParametersScaling.Instance.StrengthToHealthPoints + Stamina * CharacterParametersScaling.Instance.StaminaToHealthPoints);
        }

        public virtual void ChangeAgility(int agility)
        {
            Agility += agility;

            DodgeChance = Agility * CharacterParametersScaling.Instance.AgilityToDodgeChance;
            CriticalStrikeChance = (Strength + Agility) / 2 * CharacterParametersScaling.Instance.StrengthAndAgilityToCriticalStrikeChance;
            StaminaPoints.SetRestorationPower(StaminaPoints.MaxValue * (CharacterParametersScaling.Instance.BaseStaminaRestorationPowerPercent / 100) + Agility * CharacterParametersScaling.Instance.AgilityToStaminaRestorationPerRound);
            Piercing = Agility * CharacterParametersScaling.Instance.AgilityToPiercing;

        }

        public virtual void ChangeStamina(int stamina)
        {
            Stamina += stamina;

            HealthPoints.SetPermanentBonus(Strength * CharacterParametersScaling.Instance.StrengthToHealthPoints + Stamina * CharacterParametersScaling.Instance.StaminaToHealthPoints);
            StaminaPoints.SetPermanentBonus(Stamina * CharacterParametersScaling.Instance.StaminaToStaminaPoints);
            OnslaughtChance = Stamina * CharacterParametersScaling.Instance.StaminaToOnslaughtChance;
            ResilienceChance = Stamina * CharacterParametersScaling.Instance.StaminaToResilienceChance;
        }

        public virtual void ChangeIntelligence(int intelligence)
        {
            Intelligence += intelligence;
            int magicalDamageLevelScaling = Level / CharacterParametersScaling.Instance.LevelsCountForMultiplier * CharacterParametersScaling.Instance.LevelsToMagicalDamageMultiplier;

            MagicalDamageModifier = Intelligence * CharacterParametersScaling.Instance.IntelligenceToMagicalDamagePercent;
            MagicalDamageModifier *= magicalDamageLevelScaling > 0 ? magicalDamageLevelScaling : 1;
            MagicalHitChance = Intelligence * CharacterParametersScaling.Instance.IntelligenceToMagicalHitChance + AdvantagePercent;
            BreathPoints.SetPermanentBonus(Intelligence * CharacterParametersScaling.Instance.IntelligenceToBreathPoints);
        }

        public void ChangePhysicalDamage(int physicalDamage)
        {
            PhysicalDamageModifier += physicalDamage;
        }

        public void ChangeTotalPhysicalDamagePercent(float modifier)
        {
            TotalPhysicalDamagePercent += modifier;
        }

        public void ChangeMagicalDamage(int magicalDamage)
        {
            MagicalDamageModifier += magicalDamage;
        }

        public virtual void TakePhysicalDamage(int damage, bool isCritical)
        {
            int trueDamage = damage - (int)ArmorPoints.CurrentValue;
            if (ArmorPoints.CurrentValue > 0)
            {
                ArmorPoints.DecreaseCurrentValue(damage, isCritical);
            }
            if (trueDamage <= 0 || ArmorPoints.MinimalValue > 0)
            {
                return;
            }
            TakeTrueDamage(trueDamage, isCritical);
        }

        public virtual void TakeMagicalDamage(int damage, bool isCritical)
        {
            int trueDamage = damage - (int)BarrierPoints.CurrentValue;
            if (BarrierPoints.CurrentValue > 0)
            {
                BarrierPoints.DecreaseCurrentValue(damage, isCritical);
            }
            if (trueDamage <= 0 || BarrierPoints.MinimalValue > 0)
            {
                return;
            }
            TakeTrueDamage(trueDamage, isCritical);
        }

        public virtual void TakeTrueDamage(int damage, bool isCritical = false)
        {
            HealthPoints.DecreaseCurrentValue(damage, isCritical);
        }

        public void RestoreArmor(float restoration)
        {
            ArmorPoints.IncreaseCurrentValue(restoration);
        }

        public void RestoreBarrier(float restoration)
        {
            BarrierPoints.IncreaseCurrentValue(restoration);
        }

        public void RestoreHealth(float restoration)
        {
            HealthPoints.IncreaseCurrentValue(restoration);
        }

        public void RestoreStamina(float restoration)
        {
            StaminaPoints.IncreaseCurrentValue(restoration);
        }

        public void RestoreBreath(float restoration)
        {
            BreathPoints.IncreaseCurrentValue(restoration);
        }

        public void SetPhysicalDamageBlockPercent(int percent)
        {
            PhysicalDamageBlockPercent = percent;
        }

        public void SetMagicalDamageBlockPercent(int percent)
        {
            MagicalDamageBlockPercent = percent;
        }

        public void SetThornsPercent(int percent)
        {
            ThornsPercent = percent;
        }

        public void SetDebuffBlockPercent(int percent)
        {
            DebuffBlockPercent = percent;
        }

        public void SetAdvantagePercent(int percent)
        {
            AdvantagePercent = percent;
        }
    }
}
