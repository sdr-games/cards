using SDRGames.Whist.TalentsModule.ScriptableObjects;

namespace SDRGames.Whist.TalentsModule.Models
{
    public class Astra
    {
        public string Equipment { get; private set; }

        public Astra(AstraScriptableObject astraScriptableObject)
        {
            Equipment = astraScriptableObject.Equipment;
        }
    }
}
