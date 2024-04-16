using System;

using SDRGames.Whist.TalentsModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.TalentsEditorModule.Models
{
    [Serializable]
    public class VariableData
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public ScriptableObject Value { get; private set; }
        public enum VariableTypes { HalfAstraBonus, FullAstraBonus, FullTalamusBonus }
        [field: SerializeField] public VariableTypes Type { get; private set; }

        public VariableData(string name, ScriptableObject value, VariableTypes type)
        {
            Name = name;
            Value = value;
            Type = type;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetValue(ScriptableObject value)
        {
            Value = value;
        }

        public BonusScriptableObject SaveToSO(string folderPath)
        {
            BonusScriptableObject bonusSO;

            bonusSO = UtilityIO.CreateAsset<BonusScriptableObject>($"{folderPath}/Talents", Name);
            bonusSO.Initialize(Value);
            UtilityIO.SaveAsset(bonusSO);
            return bonusSO;
        }
    }
}
