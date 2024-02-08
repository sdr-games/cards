using System;

using SDRGames.Islands.DiceModule.Models;
using SDRGames.Islands.PointsModule.Models;

namespace SDRGames.Whist.CharacterModule.Models
{
    public class PlayerCharacterParamsModel : CommonCharacterParamsModel
    {
        private readonly string CHARACTER_NAME = "Валиор";
        private const int DEFAULT_BASE_LEVEL = 1;

        public event EventHandler<LevelChangedEventArgs> LevelChanged;
        public event EventHandler<MagicDamageMultiplierChangedEventArgs> MagicDamageMultiplierChanged;

        public PlayerCharacterParamsModel(string characterName, int level, Points healthPoints, Points staminaPoints, Points breathPoints, Points physicalArmor, Points magicShield, Dice physicalDamage, float magicDamageMultiplier) : base(characterName, level, healthPoints, staminaPoints, breathPoints, physicalArmor, magicShield, physicalDamage, magicDamageMultiplier)
        {
            CharacterName = CHARACTER_NAME;
            Level = DEFAULT_BASE_LEVEL;
        }
    }
}
