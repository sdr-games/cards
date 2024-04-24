using SDRGames.Whist.TalentsModule.ScriptableObjects;

namespace SDRGames.Whist.TalentsModule.Models
{
    public class Astra : Talent
    {
        public string Equipment { get; private set; }

        public Astra(AstraScriptableObject astraScriptableObject) : base(astraScriptableObject.Cost)
        {
            Equipment = astraScriptableObject.Equipment;
        }
    }
}
