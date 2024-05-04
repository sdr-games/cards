using UnityEngine;

namespace SDRGames.Whist.TalentsModule.ScriptableObjects
{
    public class BonusScriptableObject : ScriptableObject
    {
        public enum BonusTypes { HalfAstraBonus, FullAstraBonus, FullTalamusBonus }
        [field: SerializeField] public BonusTypes Type { get; private set; }
        [field: SerializeField] public ScriptableObject Value { get; private set; }

        public void Initialize(BonusTypes type, ScriptableObject value)
        {
            Type = type;
            Value = value;
        }
    }
}
