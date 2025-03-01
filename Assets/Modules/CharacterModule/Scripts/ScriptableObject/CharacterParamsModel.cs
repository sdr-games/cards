using System;

using SDRGames.Whist.CharacterModule.Models;
using SDRGames.Whist.DiceModule.Models;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.PointsModule.Models;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "CharacterParameters", menuName = "SDRGames/Characters/Character Parameters")]
    public class CharacterParamsModel : ScriptableObject
    {
        [field: SerializeField] public CharacterInfoScriptableObject CharacterInfo { get; protected set; }
        [field: SerializeField] public int Level { get; protected set; } = 1;

        #region Characteristics

        [field: Header("Characteristics")]
        [field: SerializeField] public int Strength { get; protected set; }
        [field: SerializeField] public int Agility { get; protected set; }
        [field: SerializeField] public int Stamina { get; protected set; }
        [field: SerializeField] public int Intelligence { get; protected set; }

        #endregion

        #region Visible Parameters

        [field: Header("Visible Parameters")]
        [field: SerializeField] public int PhysicalDamage { get; protected set; }
        [field: SerializeField] public int MagicalDamage { get; protected set; }
        [field: SerializeField] public int PhysicalHitChance { get; protected set; }
        [field: SerializeField] public int MagicalHitChance { get; protected set; }
        [field: SerializeField] public float StaminaRestorationPower { get => StaminaPoints.RestorationPower; }
        [field: SerializeField] public int Piercing { get; protected set; }

        #endregion

        #region Hidden Parameters

        [field: Header("Hidden Parameters")]
        [field: SerializeField] public int DodgeChance { get; protected set; }
        [field: SerializeField] public int BlockChance { get; protected set; }
        [field: SerializeField] public int OnslaughtChance { get; protected set; }
        [field: SerializeField] public int CriticalStrikeChance { get; protected set; }
        [field: SerializeField] public int Resilience { get; protected set; }
        [field: SerializeField] public int Weakening { get; protected set; }
        [field: SerializeField] public int Amplification { get; protected set; }
        [field: SerializeField] public Dice Initiative { get; protected set; }
        [field: SerializeField] public int StunResistance { get; protected set; }

        #endregion

        #region Points

        [field: Header("Points")]
        [field: SerializeField] public Points HealthPoints { get; protected set; }
        [field: SerializeField] public Points StaminaPoints { get; protected set; }
        [field: SerializeField] public Points BreathPoints { get; protected set; }
        [field: SerializeField] public Points ArmorPoints { get; protected set; }
        [field: SerializeField] public Points BarrierPoints { get; protected set; }

        #endregion

        #region Level changing methods

        public virtual void IncreaseLevel(int level)
        {
            Level += level;
            int physicalDamageLevelScaling = Level / CharacterParametersScaling.Instance.LevelsCountForMultiplier * CharacterParametersScaling.Instance.LevelsToPhysicalDamageMultiplier;
            int magicalDamageLevelScaling = Level / CharacterParametersScaling.Instance.LevelsCountForMultiplier * CharacterParametersScaling.Instance.LevelsToMagicalDamageMultiplier;

            PhysicalDamage = Strength * CharacterParametersScaling.Instance.StrengthToPhysicalDamage;
            PhysicalDamage *= physicalDamageLevelScaling > 0 ? physicalDamageLevelScaling : 1;
            MagicalDamage = Intelligence * CharacterParametersScaling.Instance.IntelligenceToMagicalDamage;
            MagicalDamage *= magicalDamageLevelScaling > 0 ? magicalDamageLevelScaling : 1;

            ArmorPoints.SetPermanentBonus(Level * CharacterParametersScaling.Instance.LevelToArmorPoints);
            BarrierPoints.SetPermanentBonus(Level * CharacterParametersScaling.Instance.LevelToBarrierPoints);
        }

        #endregion

        #region Characterstic changing methods

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
            Initiative = new Dice("Initiative", 1, 20 - Agility * CharacterParametersScaling.Instance.AgilityToInitiative);
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

        #endregion

        public void IncreasePhysicalDamage(int physicalDamage)
        {
            PhysicalDamage += physicalDamage;
        }

        public void IncreaseMagicalDamage(int magicalDamage)
        {
            MagicalDamage += magicalDamage;
        }

        public void TakePhysicalDamage(int damage)
        {
            float trueDamage = damage - ArmorPoints.CurrentValue;
            if (ArmorPoints.CurrentValue > 0)
            {
                ArmorPoints.DecreaseCurrentValue(damage);
            }
            if(trueDamage <= 0)
            {
                return;
            }
            TakeTrueDamage(trueDamage);
        }

        public void TakeMagicalDamage(int damage)
        {
            float trueDamage = damage - BarrierPoints.CurrentValue;
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

        public void TakeTrueDamage(float damage)
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

        private void OnEnable()
        {
            HealthPoints.SetName(nameof(HealthPoints));
            StaminaPoints.SetName(nameof(StaminaPoints));
            BreathPoints.SetName(nameof(BreathPoints));
            ArmorPoints.SetName(nameof(ArmorPoints));
            BarrierPoints.SetName(nameof(BarrierPoints));

            HealthPoints.Reset();
            StaminaPoints.Reset();
            BreathPoints.Reset();
            ArmorPoints.Reset();
            BarrierPoints.Reset();
        }
    }
}
