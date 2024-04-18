using System;

using UnityEngine;

using static SDRGames.Whist.TalentsEditorModule.Models.AstraData;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class AstraLoadedEventArgs : EventArgs
    {
        public string ID { get; private set; }
        public string NodeName { get; private set; }
        public int Cost { get; private set; }
        public EquipmentNames EquipmentName { get; private set; }

        public AstraLoadedEventArgs(string id, string nodeName, int cost, EquipmentNames equipmentName)
        {
            ID = id;
            NodeName = nodeName;
            Cost = cost;
            EquipmentName = equipmentName;
        }
    }
}