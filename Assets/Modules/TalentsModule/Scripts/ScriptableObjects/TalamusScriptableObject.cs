using System.Collections.Generic;

using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.ScriptableObjects
{
    public class TalamusScriptableObject : TalentScriptableObject
    {
        [field: SerializeField][field: ReadOnly] public string Characteristic { get; private set; }
        [field: SerializeField][field: ReadOnly] public int CharacteristicValue { get; private set; }

        public void Initialize(string dialogueName, NodeTypes dialogueType, string characteristic, int characteristicValue)
        {
            base.Initialize(dialogueName, dialogueType);
            Characteristic = characteristic;
            CharacteristicValue = characteristicValue;
        }
    }
}
