using SDRGames.Islands.DiceModule.Models;
using SDRGames.Islands.PointsModule.Models;

namespace SDRGames.Whist.CharacterModule.Models
{
    public class CommonCharacterParamsModel
    {
        public string CharacterName { get; protected set; }
        public int Level { get; protected set; }
        public Points HealthPoints { get; protected set; }
        public Points StaminaPoints { get; protected set; }
        public Points BreathPoints { get; protected set; }
        public Points PhysicalArmor { get; protected set; }
        public Points MagicShield { get; protected set; }
        public Dice PhysicalDamage { get; protected set; }
        public float MagicDamageMultiplier { get; protected set; }

        public CommonCharacterParamsModel(string characterName, int level, Points healthPoints, Points staminaPoints, Points breathPoints, Points physicalArmor, Points magicShield, Dice physicalDamage, float magicDamageMultiplier)
        {
            CharacterName = characterName;
            Level = level;
            HealthPoints = healthPoints;
            StaminaPoints = staminaPoints;
            BreathPoints = breathPoints;
            PhysicalArmor = physicalArmor;
            MagicShield = magicShield;
            PhysicalDamage = physicalDamage;
            MagicDamageMultiplier = magicDamageMultiplier;
        }
    }
}
