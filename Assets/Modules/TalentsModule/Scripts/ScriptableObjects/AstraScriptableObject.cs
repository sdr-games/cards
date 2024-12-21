using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.LocalizationModule.Models;

using UnityEngine;

using static SDRGames.Whist.TalentsModule.Models.Astra;

namespace SDRGames.Whist.TalentsModule.ScriptableObjects
{
    public class AstraScriptableObject : TalentScriptableObject
    {
        [field: SerializeField][field: ReadOnly] public EquipmentNames Equipment { get; private set; }

        public void Initialize(string name, int cost, LocalizationData descriptionLocalization, NodeTypes dialogueType, EquipmentNames equipment)
        {
            base.Initialize(name, cost, descriptionLocalization, dialogueType);
            Equipment = equipment;
        }
    }
}
