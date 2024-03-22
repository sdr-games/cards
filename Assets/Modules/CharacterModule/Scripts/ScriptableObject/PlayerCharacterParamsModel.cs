using System;

using SDRGames.Whist.DiceModule.Models;
using SDRGames.Whist.PointsModule.Models;

using UnityEngine;

namespace SDRGames.Whist.CharacterModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "PlayerParameters", menuName = "SDRGames/Dialogues/Player Parameters")]
    public class PlayerCharacterParamsModel : CommonCharacterParamsModel
    {
        private const int DEFAULT_BASE_LEVEL = 1;

        public int Experience { get; private set; }
        public int Glory { get; private set; }

        public event EventHandler<LevelChangedEventArgs> LevelChanged;
        public event EventHandler<ExperienceChangedEventArgs> ExperienceChanged;
        public event EventHandler<GloryChangedEventArgs> GloryChanged;
        public event EventHandler<MagicDamageMultiplierChangedEventArgs> MagicDamageMultiplierChanged;

        public PlayerCharacterParamsModel(CharacterInfoScriptableObject characterInfo, int level, Points healthPoints, Points staminaPoints, Points breathPoints, Points physicalArmor, Points magicShield, Dice physicalDamage, float magicDamageMultiplier, int experience, int glory) : base(characterInfo, level, healthPoints, staminaPoints, breathPoints, physicalArmor, magicShield, physicalDamage, magicDamageMultiplier)
        {
            Level = DEFAULT_BASE_LEVEL;
            Experience = experience;
            Glory = glory;
        }

        public void IncreaseLevel(int level)
        {
            Level += level;
            LevelChanged?.Invoke(this, new LevelChangedEventArgs(Level));
        }

        public void IncreaseExperience(int experience)
        {
            Experience += experience;
            ExperienceChanged?.Invoke(this, new ExperienceChangedEventArgs(Experience));
        }

        public void IncreaseGlory(int glory)
        {
            Glory += glory;
            GloryChanged?.Invoke(this, new GloryChangedEventArgs(Glory));
        }

        public void DecreaseGlory(int glory)
        {
            Glory -= glory;
            GloryChanged?.Invoke(this, new GloryChangedEventArgs(Glory));
        }
    }
}
