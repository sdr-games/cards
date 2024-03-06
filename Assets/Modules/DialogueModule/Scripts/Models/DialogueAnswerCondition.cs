using System;
using UnityEngine;

namespace SDRGames.Whist.DialogueSystem.Models
{
    [Serializable]
    public class DialogueAnswerCondition
    {
        public enum AnswerConditionTypes { CharacteristicCheck, SkillCheck};
        public enum Characteristics { Strength, Dexterity, Stamina, Constitution, Intelligence, Tenacity, Spirit, Charisma, Immunity };
        public enum SkillsNames
        {
            No = -1,
            LongSword, OneHandAxe, OneHandMace, OneHandFlail, OneHandHammer, OneHandSpear, ShortSword, Dagger,
            BastardSword, TwoHandSword, TwoHandAxe, TwoHandMace, TwoHandFlail, TwoHandHammer, TwoHandSpear, TwoHandStaff,
            Unarmed,
            Bow, LongBow, Crossbow, HeavyCrossbow, Throwing,
            Mysticism, Fire, Frost, Air, Water, Lightning, Nature, Earth, Demonology, Necromancy, Blood, Black, Holy, Vita,
            Parry, Block, Dodge, Concentration,
            Healing,
            Stealth, Pickpocket, Traps, Lockpicking,
            Attention, MagicItems, Legends, Monsters, ElderLanguages, Learnability, Search,
            Deceit, Threat, Trading, Conviction
        };

        [field: SerializeField] public AnswerConditionTypes AnswerConditionType { get; set; }
        [field: SerializeField] public Characteristics Characteristic { get; set; }
        [field: SerializeField] public SkillsNames Skill { get; set; }
        [field: SerializeField] public int RequiredValue { get; set; }
        [field: SerializeField] public bool Reversed { get; set; }

        public DialogueAnswerCondition()
        {
            Skill = SkillsNames.No;
        }
    }
}