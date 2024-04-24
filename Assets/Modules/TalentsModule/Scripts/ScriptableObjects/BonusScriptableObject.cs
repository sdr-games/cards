using UnityEngine;

namespace SDRGames.Whist.TalentsModule.ScriptableObjects
{
    public class BonusScriptableObject : ScriptableObject
    {
        public enum VariableTypes { HalfAstraBonus, FullAstraBonus, FullTalamusBonus }
        [field: SerializeField] public VariableTypes Type { get; private set; }
        [field: SerializeField] public ScriptableObject Value { get; private set; }

        public void Initialize(VariableTypes type, ScriptableObject value)
        {
            Type = type;
            Value = value;
        }
    }
}
