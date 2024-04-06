using System;

using SDRGames.Whist.TalentsModule.ScriptableObjects;

using static SDRGames.Whist.TalentsModule.ScriptableObjects.TalentScriptableObject;

namespace SDRGames.Whist.TalentsEditorModule.Models
{
    [Serializable]
    public class TalamusData : BaseData
    {
        public enum CharacteristicNames { Strength, Agility, Stamina, Intellegence, Immunity }
        public CharacteristicNames CharacteristicName { get; private set; }
        public int CharacteristicValue { get; private set; }

        public TalamusData(string name, CharacteristicNames characteristicName, int characteristicValue) : base(name)
        {
            NodeType = NodeTypes.Talamus;

            CharacteristicName = characteristicName;
            CharacteristicValue = characteristicValue;
        }

        public override void SetNodeName(string name)
        {
            NodeName = name;
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
                NodeType,
                CharacteristicName.ToString(),
                CharacteristicValue
            );
            return talamusSO;
        }
    }
}
