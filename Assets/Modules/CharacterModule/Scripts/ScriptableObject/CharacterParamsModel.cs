using System;

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
        [field: SerializeField] public int Level { get; protected set; }

        #region Characteristics

        [field: Header("Characteristics")]
        [field: SerializeField] public int Strength { get; protected set; }
        [field: SerializeField] public int Agility { get; protected set; }
        [field: SerializeField] public int Stamina { get; protected set; }
        [field: SerializeField] public int Intelligence { get; protected set; }

        #endregion

        #region Visible Parameters

        [field: Header("Visible Parameters")]
        [field: SerializeField][field: ReadOnly] public int PhysicalDamage { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int MagicDamage { get; protected set; }

        #endregion

        #region Hidden Parameters

        [field: Header("Hidden Parameters")]
        [field: SerializeField][field: ReadOnly] public int PhysicalHitChance { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int BlockChance { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int OnslaughtChance { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int CritChance { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int DodgeChance { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int Initiative { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int Resilience { get; protected set; }
        [field: SerializeField][field: ReadOnly] public int MagicalHitChance { get; protected set; }

        #endregion

        #region Points

        [field: Header("Points")]
        [field: SerializeField][field: ReadOnly] public Points HealthPoints { get; protected set; }
        [field: SerializeField][field: ReadOnly] public Points StaminaPoints { get; protected set; }
        [field: SerializeField][field: ReadOnly] public Points BreathPoints { get; protected set; }
        [field: SerializeField][field: ReadOnly] public Points ArmorPoints { get; protected set; }
        [field: SerializeField][field: ReadOnly] public Points BarrierPoints { get; protected set; }

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

            HealthPoints.SetBaseValue(Strength * Scaling.Instance.OnePointStrengthToHealthPoints + Stamina * Scaling.Instance.OnePointStaminaToHealthPoints);
            StaminaPoints.SetBaseValue(Stamina * Scaling.Instance.OnePointStaminaToStaminaPoints);
            BreathPoints.SetBaseValue(Intelligence * Scaling.Instance.OnePointIntelligenceToBreathPoints);
            ArmorPoints.SetBaseValue(Level * Scaling.Instance.OneLevelToArmorPoints);
            BarrierPoints.SetBaseValue(Level * Scaling.Instance.OneLevelToBarrierPoints);

            PhysicalDamage = Strength * Scaling.Instance.OnePointStrengthToPhysicalDamage;
            PhysicalHitChance = Strength * Scaling.Instance.OnePointStrengthToHitChance;

            Initiative = Agility * Scaling.Instance.OnePointAgilityToInitiative;


            MagicDamage = Intelligence * Scaling.Instance.OnePointIntelligenceToMagicalDamage;
        }
    }
}
