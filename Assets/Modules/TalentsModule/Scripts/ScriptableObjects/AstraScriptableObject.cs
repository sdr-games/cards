using System;

using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.LocalizationModule.Models;

using UnityEngine;

namespace SDRGames.Whist.TalentsModule.ScriptableObjects
{
    public class AstraScriptableObject : TalentScriptableObject
    {
        [field: SerializeField][field: ReadOnly] public string Equipment { get; private set; }

        public void Initialize(string name, int cost, LocalizationData descriptionLocalization, NodeTypes dialogueType, string equipment)
        {
            base.Initialize(name, cost, descriptionLocalization, dialogueType);
            Equipment = equipment;
        }
    }
}
