using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.TalentsModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.Models
{
    public class Talamus : Talent
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
