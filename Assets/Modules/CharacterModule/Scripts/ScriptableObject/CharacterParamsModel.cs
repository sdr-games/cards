using System;

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
        [field: SerializeField] public Points HealthPoints { get; protected set; }
        [field: SerializeField] public Points StaminaPoints { get; protected set; }
        [field: SerializeField] public Points BreathPoints { get; protected set; }
        [field: SerializeField] public Points Armor { get; protected set; }
        [field: SerializeField] public Points Barrier { get; protected set; }
        [field: SerializeField] public Dice PhysicalDamage { get; protected set; }
        [field: SerializeField] public float MagicDamageMultiplier { get; protected set; }

        public CharacterParamsModel(CharacterInfoScriptableObject characterInfo, int level, Points healthPoints, Points staminaPoints, Points breathPoints, Points physicalArmor, Points magicShield, Dice physicalDamage, float magicDamageMultiplier)
        {
            CharacterInfo = characterInfo;
            Level = level;
            HealthPoints = healthPoints;
            StaminaPoints = staminaPoints;
            BreathPoints = breathPoints;
            Armor = physicalArmor;
            Barrier = magicShield;
            PhysicalDamage = physicalDamage;
            MagicDamageMultiplier = magicDamageMultiplier;
        }

        public void TakeDamage(int damage)
        {
            float trueDamage = damage - Armor.CurrentValue;
            Armor.DecreaseCurrentValue(damage);
            if(trueDamage <= 0)
            {
                return;
            }
            HealthPoints.DecreaseCurrentValue(trueDamage);
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
