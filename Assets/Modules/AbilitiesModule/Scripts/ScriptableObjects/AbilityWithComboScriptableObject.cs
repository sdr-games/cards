using UnityEngine;

namespace SDRGames.Whist.AbilitiesModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "AbilityWithCombo", menuName = "SDRGames/Combat/AbilityWithCombo")]
    public class AbilityWithComboScriptableObject : AbilityScriptableObject
    {
        [field: SerializeField] public AbilityComboScriptableObject[] AbilityComboScriptableObjects { get; private set; }
    }
}
