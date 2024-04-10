using static SDRGames.Whist.TalentsEditorModule.Models.AstraData;

namespace SDRGames.Whist.TalentsEditorModule.Views
{
    public class AstraLoadedEventArgs
    {
        public EquipmentNames EquipmentName { get; private set; }

        public AstraLoadedEventArgs(EquipmentNames equipmentName)
        {
            EquipmentName = equipmentName;
        }
    }
}