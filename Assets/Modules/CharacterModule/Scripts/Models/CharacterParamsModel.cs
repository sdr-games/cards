using SDRGames.Whist.CharacterModule.ScriptableObjects;
using SDRGames.Whist.PointsModule.Models;

namespace SDRGames.Whist.CharacterModule.Models
{
    public class CharacterParamsModel
    {
        public int Level { get; protected set; }
        public int Strength { get; protected set; }
        public int Agility { get; protected set; }
        public int Stamina { get; protected set; }
        public int Intelligence { get; protected set; }
        public int PhysicalDamage { get; protected set; }
        public int MagicalDamage { get; protected set; }
        public int PhysicalHitChance { get; protected set; }
        public int MagicalHitChance { get; protected set; }
        public float StaminaRestorationPower { get => StaminaPoints.RestorationPower; }
        public int Piercing { get; protected set; }
        public int DodgeChance { get; protected set; }
        public int BlockChance { get; protected set; }
        public int OnslaughtChance { get; protected set; }
        public int CriticalStrikeChance { get; protected set; }
        public int Resilience { get; protected set; }
        public int Weakening { get; protected set; }
        public int Amplification { get; protected set; }
        public int StunResistance { get; protected set; }
        public Points HealthPoints { get; protected set; }
        public Points StaminaPoints { get; protected set; }
        public Points BreathPoints { get; protected set; }
        public Points ArmorPoints { get; protected set; }
        public Points BarrierPoints { get; protected set; }

        public CharacterParamsModel(CharacterParamsScriptableObject characterParamsScriptableObject)
        {
            Level = characterParamsScriptableObject.Level;
            Strength = characterParamsScriptableObject.Strength;
            Agility = characterParamsScriptableObject.Agility;
            Stamina = characterParamsScriptableObject.Stamina;
            Intelligence = characterParamsScriptableObject.Intelligence;

            PhysicalDamage = characterParamsScriptableObject.PhysicalDamage;
            MagicalDamage = characterParamsScriptableObject.MagicalDamage;
            PhysicalHitChance = characterParamsScriptableObject.PhysicalHitChance;
            MagicalHitChance = characterParamsScriptableObject.MagicalHitChance;
            Piercing = characterParamsScriptableObject.Piercing;
            DodgeChance = characterParamsScriptableObject.DodgeChance;
            BlockChance = characterParamsScriptableObject.BlockChance;
            OnslaughtChance = characterParamsScriptableObject.OnslaughtChance;
            CriticalStrikeChance = characterParamsScriptableObject.CriticalStrikeChance;
            Resilience = characterParamsScriptableObject.Resilience;
            Weakening = characterParamsScriptableObject.Weakening;
            Amplification = characterParamsScriptableObject.Amplification;
            StunResistance = characterParamsScriptableObject.StunResistance;

            HealthPoints = new Points(characterParamsScriptableObject.HealthPoints);
            StaminaPoints = new Points(characterParamsScriptableObject.StaminaPoints);
            BreathPoints = new Points(characterParamsScriptableObject.BreathPoints);
            ArmorPoints = new Points(characterParamsScriptableObject.ArmorPoints);
            BarrierPoints = new Points(characterParamsScriptableObject.BarrierPoints);
        }

        public virtual void IncreaseStrength(int strength)
        {
            Strength += strength;
            int physicalDamageLevelScaling = Level / CharacterParametersScaling.Instance.LevelsCountForMultiplier * CharacterParametersScaling.Instance.LevelsToPhysicalDamageMultiplier;

            PhysicalDamage = Strength * CharacterParametersScaling.Instance.StrengthToPhysicalDamage;
            PhysicalDamage *= physicalDamageLevelScaling > 0 ? physicalDamageLevelScaling : 1;
            PhysicalHitChance = Strength * CharacterParametersScaling.Instance.StrengthToPhysicalHitChance;
            BlockChance = Strength * CharacterParametersScaling.Instance.StrengthToBlockChance;
            CriticalStrikeChance = (Strength + Agility) / 2 * CharacterParametersScaling.Instance.StrengthAndAgilityToCriticalStrikeChance;
            HealthPoints.SetPermanentBonus(Strength * CharacterParametersScaling.Instance.StrengthToHealthPoints + Stamina * CharacterParametersScaling.Instance.StaminaToHealthPoints);
        }

        public virtual void IncreaseAgility(int agility)
        {
            Agility += agility;

            DodgeChance = Agility * CharacterParametersScaling.Instance.AgilityToDodgeChance;
            CriticalStrikeChance = (Strength + Agility) / 2 * CharacterParametersScaling.Instance.StrengthAndAgilityToCriticalStrikeChance;
            StaminaPoints.SetRestorationPower(StaminaPoints.MaxValue * (CharacterParametersScaling.Instance.BaseStaminaRestorationPowerPercent / 100) + Agility * CharacterParametersScaling.Instance.AgilityToStaminaRestorationPerRound);
            Piercing = Agility * CharacterParametersScaling.Instance.AgilityToPiercing;

        }

        public virtual void IncreaseStamina(int stamina)
        {
            Stamina += stamina;

            HealthPoints.SetPermanentBonus(Strength * CharacterParametersScaling.Instance.StrengthToHealthPoints + Stamina * CharacterParametersScaling.Instance.StaminaToHealthPoints);
            StaminaPoints.SetPermanentBonus(Stamina * CharacterParametersScaling.Instance.StaminaToStaminaPoints);
            OnslaughtChance = Stamina * CharacterParametersScaling.Instance.StaminaToOnslaughtChance;
            Resilience = Stamina * CharacterParametersScaling.Instance.StaminaToResilience;
        }

        public virtual void IncreaseIntelligence(int intelligence)
        {
            Intelligence += intelligence;
            int magicalDamageLevelScaling = Level / CharacterParametersScaling.Instance.LevelsCountForMultiplier * CharacterParametersScaling.Instance.LevelsToMagicalDamageMultiplier;

            MagicalDamage = Intelligence * CharacterParametersScaling.Instance.IntelligenceToMagicalDamage;
            MagicalDamage *= magicalDamageLevelScaling > 0 ? magicalDamageLevelScaling : 1;
            MagicalHitChance = Intelligence * CharacterParametersScaling.Instance.IntelligenceToMagicalHitChance;
            BreathPoints.SetPermanentBonus(Intelligence * CharacterParametersScaling.Instance.IntelligenceToBreathPoints);
        }

        public void IncreasePhysicalDamage(int physicalDamage)
        {
            PhysicalDamage += physicalDamage;
        }

        public void IncreaseMagicalDamage(int magicalDamage)
        {
            MagicalDamage += magicalDamage;
        }

        public virtual void TakePhysicalDamage(int damage)
        {
            int trueDamage = damage - (int)ArmorPoints.CurrentValue;
            if (ArmorPoints.CurrentValue > 0)
            {
                ArmorPoints.DecreaseCurrentValue(damage);
            }
            if (trueDamage <= 0)
            {
                return;
            }
            TakeTrueDamage(trueDamage);
        }

        public virtual void TakeMagicalDamage(int damage)
        {
            int trueDamage = damage - (int)BarrierPoints.CurrentValue;
            if (BarrierPoints.CurrentValue > 0)
            {
                BarrierPoints.DecreaseCurrentValue(damage);
            }
            if (trueDamage <= 0)
            {
                return;
            }
            TakeTrueDamage(trueDamage);
        }

        public virtual void TakeTrueDamage(int damage)
        {
            HealthPoints.DecreaseCurrentValue(damage);
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
    }
}
