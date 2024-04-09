using System;

using SDRGames.Whist.TalentsModule.ScriptableObjects;

using static SDRGames.Whist.TalentsModule.ScriptableObjects.TalentScriptableObject;

namespace SDRGames.Whist.TalentsEditorModule.Models
{
    public class AstraData : BaseData
    {
        public enum EquipmentNames { Weapon }
        public EquipmentNames Equipment { get; private set; }

        public AstraData(string name) : base(name)
        {
            NodeType = NodeTypes.Astra;

            Equipment = default;
        }

        public void Load(EquipmentNames equipment)
        {
            Equipment = equipment;
        }

        public override void SetNodeName(string name)
        {
            NodeName = name;
        }

        public void SetEquipment(string equipment)
        {
            Equipment = (EquipmentNames)Enum.Parse(typeof(EquipmentNames), equipment);
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
