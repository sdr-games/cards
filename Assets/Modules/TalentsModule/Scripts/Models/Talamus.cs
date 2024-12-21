using SDRGames.Whist.TalentsModule.ScriptableObjects;

namespace SDRGames.Whist.TalentsModule.Models
{
    public class Talamus : Talent
    {
        public enum CharacteristicNames { Strength, Agility, Stamina, Intellegence }
        public CharacteristicNames Characteristic { get; private set; }
        public int CharacteristicValuePerPoint { get; private set; }

        public Talamus(TalamusScriptableObject talamusScriptableObject) : base(talamusScriptableObject.Cost, talamusScriptableObject.DescriptionLocalization.GetLocalizedText())
        {
            Characteristic = talamusScriptableObject.Characteristic;
            CharacteristicValuePerPoint = talamusScriptableObject.CharacteristicValuePerPoint;
        }
    }
}
