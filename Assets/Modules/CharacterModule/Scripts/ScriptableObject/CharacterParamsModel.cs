using System;

using SDRGames.Whist.DiceModule.Models;
using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.PointsModule.Models;
using SDRGames.Whist.SettingsModule.Models;

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
        [field: SerializeField] public int Strength { get; protected set; } = 1;
        [field: SerializeField] public int Agility { get; protected set; } = 1;
        [field: SerializeField] public int Stamina { get; protected set; } = 1;
        [field: SerializeField] public int Intelligence { get; protected set; } = 1;

        #endregion

        #region Visible Parameters

        [field: Header("Visible Parameters")]
        [field: SerializeField][field: ReadOnly] public int PhysicalDamage { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int MagicalDamage { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int PhysicalHitChance { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int MagicalHitChance { get; protected set; }
        [field: SerializeField][field: ReadOnly] public float StaminaRestorationPower { get => StaminaPoints.RestorationPower; }
        [field: SerializeField][field: ReadOnly] public int Piercing { get; protected set; }

        #endregion

        #region Hidden Parameters

        [field: Header("Hidden Parameters")]
        [field: SerializeField][field: ReadOnly] public int DodgeChance { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int BlockChance { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int OnslaughtChance { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int CriticalStrikeChance { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int Resilience { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int Weakening { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int Amplification { get; protected set; }
        [field: SerializeField][field: ReadOnly] public Dice Initiative { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int StunResistance { get; protected set; }

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
            int physicalDamageLevelScaling = Level / Scaling.Instance.LevelsCountForMultiplier * Scaling.Instance.LevelsToPhysicalDamageMultiplier;
            int magicalDamageLevelScaling = Level / Scaling.Instance.LevelsCountForMultiplier * Scaling.Instance.LevelsToMagicalDamageMultiplier;

            PhysicalDamage = Strength * Scaling.Instance.StrengthToPhysicalDamage;
            PhysicalDamage *= physicalDamageLevelScaling > 0 ? physicalDamageLevelScaling : 1;
            MagicalDamage = Intelligence * Scaling.Instance.IntelligenceToMagicalDamage;
            MagicalDamage *= magicalDamageLevelScaling > 0 ? magicalDamageLevelScaling : 1;

            ArmorPoints.SetPermanentBonus(Level * Scaling.Instance.LevelToArmorPoints);
            BarrierPoints.SetPermanentBonus(Level * Scaling.Instance.LevelToBarrierPoints);
        }

        #endregion

        #region Characterstic changing methods

        public virtual void ChangeStrength(int strength)
        {
            Strength += strength;
            if(Strength < 1)
            {
                Strength = 1;
            }
            int physicalDamageLevelScaling = Level / Scaling.Instance.LevelsCountForMultiplier * Scaling.Instance.LevelsToPhysicalDamageMultiplier;

            PhysicalDamage = Strength * Scaling.Instance.StrengthToPhysicalDamage;
            PhysicalDamage *= physicalDamageLevelScaling > 0 ? physicalDamageLevelScaling : 1;
            PhysicalHitChance = Strength * Scaling.Instance.StrengthToPhysicalHitChance;
            BlockChance = Strength * Scaling.Instance.StrengthToBlockChance;
            CriticalStrikeChance = (Strength + Agility) / 2 * Scaling.Instance.StrengthAndAgilityToCriticalStrikeChance;
            HealthPoints.SetPermanentBonus(Strength * Scaling.Instance.StrengthToHealthPoints + Stamina * Scaling.Instance.StaminaToHealthPoints);
        }

        public virtual void ChangeAgility(int agility)
        {
            Agility += agility;
            if(Agility < 1)
            {
                Agility = 1;
            }

            DodgeChance = Agility * Scaling.Instance.AgilityToDodgeChance;
            CriticalStrikeChance = (Strength + Agility) / 2 * Scaling.Instance.StrengthAndAgilityToCriticalStrikeChance;
            StaminaPoints.SetRestorationPower(StaminaPoints.MaxValue * (Scaling.Instance.BaseStaminaRestorationPowerPercent / 100) + Agility * Scaling.Instance.AgilityToStaminaRestorationPerRound);
            Initiative = new Dice("Initiative", 1, 20 - Agility * Scaling.Instance.AgilityToInitiative);
            Piercing = Agility * Scaling.Instance.AgilityToPiercing;

        }

        public virtual void ChangeStamina(int stamina)
        {
            Stamina += stamina;
            if(Stamina < 1)
            {
                Stamina = 1;
            } 

            HealthPoints.SetPermanentBonus(Strength * Scaling.Instance.StrengthToHealthPoints + Stamina * Scaling.Instance.StaminaToHealthPoints);
            StaminaPoints.SetPermanentBonus(Stamina * Scaling.Instance.StaminaToStaminaPoints);
            OnslaughtChance = Stamina * Scaling.Instance.StaminaToOnslaughtChance;
            Resilience = Stamina * Scaling.Instance.StaminaToResilience;
        }

        public virtual void ChangeIntelligence(int intelligence)
        {
            Intelligence += intelligence;
            if(Intelligence < 1)
            {
                Intelligence = 1;
            } 
            int magicalDamageLevelScaling = Level / Scaling.Instance.LevelsCountForMultiplier * Scaling.Instance.LevelsToMagicalDamageMultiplier;

            MagicalDamage = Intelligence * Scaling.Instance.IntelligenceToMagicalDamage;
            MagicalDamage *= magicalDamageLevelScaling > 0 ? magicalDamageLevelScaling : 1;
            MagicalHitChance = Intelligence * Scaling.Instance.IntelligenceToMagicalHitChance;
            BreathPoints.SetPermanentBonus(Intelligence * Scaling.Instance.IntelligenceToBreathPoints);
        }

        #endregion

        public void TakePhysicalDamage(int damage)
        {
            float trueDamage = damage - ArmorPoints.CurrentValue;
            ArmorPoints.DecreaseCurrentValue(damage);
            if(trueDamage <= 0)
            {
                return;
            }
            TakeTrueDamage(trueDamage);
        }

        public void TakeMagicalDamage(int damage)
        {
            float trueDamage = damage - BarrierPoints.CurrentValue;
            BarrierPoints.DecreaseCurrentValue(damage);
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

        public void RestoreArmor(int restoration)
        {
            ArmorPoints.IncreaseCurrentValue(restoration);
        }

        public void RestoreBarrier(int restoration)
        {
            BarrierPoints.IncreaseCurrentValue(restoration);
        }

        public void RestoreHealth(int restoration)
        {
            HealthPoints.IncreaseCurrentValue(restoration);
        }

        public void RestoreStamina(int restoration)
        {
            StaminaPoints.IncreaseCurrentValue(restoration);
        }

        public void RestoreBreath(int restoration)
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

        private void OnValidate()
        {
            if(Strength <= 0)
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

        protected void OnDisable()
        {
            Level = 1;
            Strength = 1;
            Agility = 1;
            Stamina = 1;
            Intelligence = 1;
            CalculateParameters();
        }

        protected void CalculateParameters()
        {
            int physicalDamageLevelScaling = Level / Scaling.Instance.LevelsCountForMultiplier * Scaling.Instance.LevelsToPhysicalDamageMultiplier;
            int magicalDamageLevelScaling = Level / Scaling.Instance.LevelsCountForMultiplier * Scaling.Instance.LevelsToMagicalDamageMultiplier;

            PhysicalDamage = Strength * Scaling.Instance.StrengthToPhysicalDamage;
            PhysicalDamage *= physicalDamageLevelScaling > 0 ? physicalDamageLevelScaling : 1;
            MagicalDamage = Intelligence * Scaling.Instance.IntelligenceToMagicalDamage;
            MagicalDamage *= magicalDamageLevelScaling > 0 ? magicalDamageLevelScaling : 1;
            PhysicalHitChance = Strength * Scaling.Instance.StrengthToPhysicalHitChance;
            MagicalHitChance = Intelligence * Scaling.Instance.IntelligenceToMagicalHitChance;
            Piercing = Agility * Scaling.Instance.AgilityToPiercing;

            DodgeChance = Agility * Scaling.Instance.AgilityToDodgeChance;
            BlockChance = Strength * Scaling.Instance.StrengthToBlockChance;
            OnslaughtChance = Stamina * Scaling.Instance.StaminaToOnslaughtChance;
            CriticalStrikeChance = (Strength + Agility) / 2 * Scaling.Instance.StrengthAndAgilityToCriticalStrikeChance;
            Resilience = Stamina * Scaling.Instance.StaminaToResilience;
            Weakening = 0;
            Amplification = 0;
            Initiative = new Dice("Initiative", 1, 21 - Agility * Scaling.Instance.AgilityToInitiative);

            HealthPoints.SetPermanentBonus(Strength * Scaling.Instance.StrengthToHealthPoints + Stamina * Scaling.Instance.StaminaToHealthPoints);
            StaminaPoints.SetPermanentBonus(Stamina * Scaling.Instance.StaminaToStaminaPoints);
            BreathPoints.SetPermanentBonus(Intelligence * Scaling.Instance.IntelligenceToBreathPoints);
            ArmorPoints.SetPermanentBonus(Level * Scaling.Instance.LevelToArmorPoints);
            BarrierPoints.SetPermanentBonus(Level * Scaling.Instance.LevelToBarrierPoints);

            StaminaPoints.SetRestorationPower(StaminaPoints.MaxValue * (Scaling.Instance.BaseStaminaRestorationPowerPercent / 100) + Agility * Scaling.Instance.AgilityToStaminaRestorationPerRound);
            StunResistance = ArmorPoints.CurrentValue > 0 ? Scaling.Instance.BaseStunResistance : Scaling.Instance.BaseStunResistanceWithoutArmor;
        }
    }
}
