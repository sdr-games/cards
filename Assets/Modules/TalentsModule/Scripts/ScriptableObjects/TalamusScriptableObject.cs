using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.LocalizationModule.Models;

using UnityEngine;

using static SDRGames.Whist.TalentsModule.Models.Talamus;

namespace SDRGames.Whist.TalentsModule.ScriptableObjects
{
    public class TalamusScriptableObject : TalentScriptableObject
    {
        [field: SerializeField][field: ReadOnly] public CharacteristicNames Characteristic { get; private set; }
        [field: SerializeField][field: ReadOnly] public int CharacteristicValuePerPoint { get; private set; }

        public void Initialize(string name, int cost, LocalizationData descriptionLocalization, NodeTypes talentType, CharacteristicNames characteristic, int characteristicValue)
        {
            base.Initialize(name, cost, descriptionLocalization, talentType);
            Characteristic = characteristic;
            CharacteristicValuePerPoint = characteristicValue;
        }
    }
}
