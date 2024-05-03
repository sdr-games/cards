using System;

using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.ScriptableObjects
{
    public class TalamusScriptableObject : TalentScriptableObject
    {
        [field: SerializeField][field: ReadOnly] public string Characteristic { get; private set; }
        [field: SerializeField][field: ReadOnly] public int CharacteristicValue { get; private set; }

        public void Initialize(string name, int cost, NodeTypes talentType, string characteristic, int characteristicValue)
        {
            base.Initialize(name, cost, talentType);
            Characteristic = characteristic;
            CharacteristicValue = characteristicValue;
        }
    }
}
