using System;

using SDRGames.Whist.TalentsModule.ScriptableObjects;

using static SDRGames.Whist.TalentsModule.ScriptableObjects.TalentScriptableObject;

namespace SDRGames.Whist.TalentsEditorModule.Models
{
    public class AstraData : BaseData
    {
        public enum EquipmentName { Weapon }
        public EquipmentName Equipment { get; private set; }

        public AstraData(string name, EquipmentName equipment) : base(name)
        {
            NodeType = NodeTypes.Astra;

            Equipment = equipment;
        }

        public override void SetNodeName(string name)
        {
            NodeName = name;
        }
        public void SetEquipment(string equipment)
        {
            Equipment = (EquipmentName)Enum.Parse(typeof(EquipmentName), equipment);
        }

        public AstraScriptableObject SaveToSO(AstraScriptableObject astraSO)
        {
            astraSO.Initialize(
                NodeName,
                NodeType,
                Equipment.ToString()
            );
            UtilityIO.SaveAsset(astraSO);
            return astraSO;
        }
    }
}
