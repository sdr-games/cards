using SDRGames.Whist.TalentsModule.ScriptableObjects;

namespace SDRGames.Whist.TalentsModule.Models
{
    public class Talamus
    {
        public string Characteristic { get; private set; }
        public int CharacteristicValue { get; private set; }

        public Talamus(TalamusScriptableObject talamusScriptableObject)
        {
            Characteristic = talamusScriptableObject.Characteristic;
            CharacteristicValue = talamusScriptableObject.CharacteristicValue;
        }
    }
}
