using System;

using SDRGames.Whist.DiceModule.Models;
using SDRGames.Whist.PointsModule.Models;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.Models
{
    [Serializable]
    public class CommonCharacterParamsModel
    {
        [field: SerializeField] public string CharacterName { get; protected set; }
        [field: SerializeField] public Sprite CharacterPortrait { get; protected set; }
        [field: SerializeField] public int Level { get; protected set; }
        [field: SerializeField] public Points HealthPoints { get; protected set; }
        [field: SerializeField] public Points StaminaPoints { get; protected set; }
        [field: SerializeField] public Points BreathPoints { get; protected set; }
        [field: SerializeField] public Points Armor { get; protected set; }
        [field: SerializeField] public Points Barrier { get; protected set; }
        [field: SerializeField] public Dice PhysicalDamage { get; protected set; }
        [field: SerializeField] public float MagicDamageMultiplier { get; protected set; }

        public CommonCharacterParamsModel(string characterName, int level, Points healthPoints, Points staminaPoints, Points breathPoints, Points physicalArmor, Points magicShield, Dice physicalDamage, float magicDamageMultiplier)
        {
            CharacterName = characterName;
            Level = level;
            HealthPoints = healthPoints;
            StaminaPoints = staminaPoints;
            BreathPoints = breathPoints;
            Armor = physicalArmor;
            Barrier = magicShield;
            PhysicalDamage = physicalDamage;
            MagicDamageMultiplier = magicDamageMultiplier;
        }
    }
}
