using SDRGames.Islands.DiceModule;
using SDRGames.Islands.PointsModule.Model;

namespace SDRGames.Whist.CharacterModule.Models
{
    public class CommonCharacterParamsModel
    {
        public string CharacterName { get; private set; }
        public int Level { get; private set; }
        public Points HealthPoints { get; private set; }
        public Points StaminaPoints { get; private set; }
        public Points BreathPoints { get; private set; }
        public Points PhysicalArmor { get; private set; }
        public Points MagicShield { get; private set; }
        public Dice PhysicalDamage { get; private set; }
        public float MagicDamageMultiplier { get; private set; }

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
