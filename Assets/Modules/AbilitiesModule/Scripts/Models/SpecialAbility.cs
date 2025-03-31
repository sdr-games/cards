using System;

using SDRGames.Whist.AbilitiesModule.ScriptableObjects;

namespace SDRGames.Whist.AbilitiesModule.Models
{
    [Serializable]
    public class SpecialAbility : Ability
    {
        public int TotalCooldown { get; private set; }
        public int CurrentCooldown { get; private set; }

        public SpecialAbility(SpecialAbilityScriptableObject specialAbilityScriptableObject) : base(specialAbilityScriptableObject)
        {
            TotalCooldown = specialAbilityScriptableObject.Cooldown;
            CurrentCooldown = 0;
        }

        public void SetCooldown()
        {
            CurrentCooldown = TotalCooldown;
        }

        public void DecreaseCooldown(int cooldown = 1)
        {
            if (CurrentCooldown > 0)
            {
                CurrentCooldown -= cooldown;
            }
        }
    }
}
