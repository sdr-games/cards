using System;

using SDRGames.Whist.CharacterModule.Presenters;
using SDRGames.Whist.DiceModule.Models;
using SDRGames.Whist.PointsModule.Models;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "CharacterParameters", menuName = "SDRGames/Characters/Character Parameters")]
    public class CharacterParamsModel : ScriptableObject
    {
        [field: SerializeField] public CharacterInfoScriptableObject CharacterInfo { get; protected set; }
        [field: SerializeField] public int Level { get; protected set; }
        [field: SerializeField] public int Initiative { get; protected set; }
        [field: SerializeField] public Points HealthPoints { get; protected set; }
        [field: SerializeField] public Points StaminaPoints { get; protected set; }
        [field: SerializeField] public Points BreathPoints { get; protected set; }
        [field: SerializeField] public Points Armor { get; protected set; }
        [field: SerializeField] public Points Barrier { get; protected set; }
        [field: SerializeField] public Dice PhysicalDamage { get; protected set; }
        [field: SerializeField] public float MagicDamageMultiplier { get; protected set; }

        public void TakePhysicalDamage(int damage)
        {
            float trueDamage = damage - Armor.CurrentValue;
            Armor.DecreaseCurrentValue(damage);
            if(trueDamage <= 0)
            {
                return;
            }
            TakeTrueDamage(trueDamage);
        }

        public void TakeMagicalDamage(int damage)
        {
            float trueDamage = damage - Barrier.CurrentValue;
            Barrier.DecreaseCurrentValue(damage);
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
            Armor.IncreaseCurrentValue(restoration);
        }

        public void RestoreBarrier(int restoration)
        {
            Barrier.IncreaseCurrentValue(restoration);
        }

        public void RestoreHealth(int restoration)
        {
            HealthPoints.IncreaseCurrentValue(restoration);
        }

        private void OnEnable()
        {
            HealthPoints.Reset();
            StaminaPoints.Reset();
            BreathPoints.Reset();
            Armor.Reset();
            Barrier.Reset();
        }
    }
}
