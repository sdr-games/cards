using UnityEngine;

namespace SDRGames.Whist.TalentsModule.ScriptableObjects
{
    public class BonusScriptableObject : ScriptableObject
    {
        [field: SerializeField] public ScriptableObject Value { get; private set; }

        public void Initialize(ScriptableObject value)
        {
            Value = value;
        }
    }
}
