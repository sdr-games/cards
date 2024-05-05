using System;

using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.LocalizationModule.Models;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.ScriptableObjects
{
    public class TalamusScriptableObject : TalentScriptableObject
    {
        [field: SerializeField][field: ReadOnly] public string Characteristic { get; private set; }
        [field: SerializeField][field: ReadOnly] public int CharacteristicValue { get; private set; }

        public void Initialize(string name, int cost, LocalizationData descriptionLocalization, NodeTypes talentType, string characteristic, int characteristicValue)
        {
            base.Initialize(name, cost, descriptionLocalization, talentType);
            Characteristic = characteristic;
            CharacteristicValue = characteristicValue;
        }
    }
}
