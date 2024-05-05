using System;

using SDRGames.Whist.TalentsModule.ScriptableObjects;

using UnityEngine;

using static SDRGames.Whist.TalentsModule.ScriptableObjects.BonusScriptableObject;

namespace SDRGames.Whist.TalentsEditorModule.Models
{
    [Serializable]
    public class BonusData
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public ScriptableObject Value { get; private set; }
        [field: SerializeField] public BonusTypes Type { get; private set; }

        public BonusData(string name, ScriptableObject value, BonusTypes type)
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
            bonusSO.Initialize(Type, Value);
            UtilityIO.SaveAsset(bonusSO);
            return bonusSO;
        }
    }
}
