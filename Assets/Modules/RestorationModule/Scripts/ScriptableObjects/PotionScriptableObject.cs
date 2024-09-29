using SDRGames.Whist.AbilitiesQueueModule.ScriptableObjects;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.RestorationModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PotionScriptableObject", menuName = "SDRGames/Items/Potion")]
    public class PotionScriptableObject : AbilityScriptableObject
    {
        public string GetEffectDescription()
        {
            return "";
        }
    }
}
