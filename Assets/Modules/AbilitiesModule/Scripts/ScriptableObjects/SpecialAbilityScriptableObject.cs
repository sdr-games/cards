using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SpecialAbility", menuName = "SDRGames/Combat/Special Ability")]
    public class SpecialAbilityScriptableObject : AbilityScriptableObject
    {
        [field: SerializeField] public int Cooldown { get; private set; }

        protected override void OnEnable()
        {
            this.CheckFieldValueIsNotNull(nameof(Cooldown), Cooldown);
            base.OnEnable();
        }
    }
}
