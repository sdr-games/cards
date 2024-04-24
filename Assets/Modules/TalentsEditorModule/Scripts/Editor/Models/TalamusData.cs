using System;

using SDRGames.Whist.TalentsEditorModule.Views;
using SDRGames.Whist.TalentsModule.Models;
using SDRGames.Whist.TalentsModule.ScriptableObjects;

using UnityEditor;

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

        public void Load(TalamusLoadedEventArgs data) 
        {
            ID = data.ID;
            NodeName = data.NodeName;
            Cost = data.Cost;
            CharacteristicName = data.CharacteristicName;
            CharacteristicValue = data.CharacteristicValue;
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
            EditorUtility.SetDirty(talamusSO);
            UtilityIO.SaveAsset(talamusSO);
            return talamusSO;
        }
    }
}
