using SDRGames.Whist.TalentsModule.ScriptableObjects;

namespace SDRGames.Whist.TalentsModule.Models
{
    public class Astra : Talent
    {
        public enum EquipmentNames { Weapon }
        public EquipmentNames Equipment { get; private set; }

        public Astra(AstraScriptableObject astraScriptableObject) : base(astraScriptableObject.Cost, astraScriptableObject.DescriptionLocalization.GetLocalizedText())
        {
            Equipment = astraScriptableObject.Equipment;
        }
    }
}
