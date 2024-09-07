using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.RestorationModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PotionScriptableObject", menuName = "SDRGames/Items/Potion")]
    public class PotionScriptableObject : AbilityScriptableObject
    {
        [field: SerializeField] public ScriptableObject[] AbilityLogics { get; private set; }

        public string GetEffectDescription()
        {
            return "";
        }
    }
}
