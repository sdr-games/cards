using System;

using SDRGames.Whist.TalentsModule.ScriptableObjects;

using UnityEngine;

using static SDRGames.Whist.TalentsModule.ScriptableObjects.TalentScriptableObject;

namespace SDRGames.Whist.TalentsEditorModule.Models
{
    [Serializable]
    public class TalamusData : BaseData
    {
        public enum CharacteristicNames { Strength, Agility, Stamina, Intellegence, Immunity }
        public CharacteristicNames CharacteristicName { get; private set; }
        public int CharacteristicValue { get; private set; }

        public TalamusData(string name) : base(name)
        {
            NodeType = NodeTypes.Talamus;

            CharacteristicName = default;
            CharacteristicValue = 0;
        }

        public void Load(CharacteristicNames characteristicName, int characteristicValue) 
        {
            CharacteristicName = characteristicName;
            CharacteristicValue = characteristicValue;
        }

        public void SetCharacteristicName(CharacteristicNames characteristicName)
        {
            CharacteristicName = characteristicName;
        }

        public void SetCharacteristicName(string characteristicName)
        {
            CharacteristicName = (CharacteristicNames)Enum.Parse(typeof(CharacteristicNames), characteristicName);
        }

        public void SetCharacteristicValue(int value)
        {
            CharacteristicValue = value;
        }

        public TalamusScriptableObject SaveToSO(TalamusScriptableObject talamusSO)
        {
            talamusSO.Initialize(
                NodeName,
                Cost,
                NodeType,
                CharacteristicName.ToString(),
                CharacteristicValue
            );
            return talamusSO;
        }
    }
}
