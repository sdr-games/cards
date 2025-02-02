using SDRGames.Whist.AbilitiesModule.Models;
using SDRGames.Whist.RestorationModule.ScriptableObjects;

namespace SDRGames.Whist.RestorationModule.Models
{
    public class Potion : Ability
    {
        public Potion(PotionScriptableObject potionScriptableObject) : base(potionScriptableObject)
        {
        }

        public string GetEffectDescription()
        {
            return "";
        }
    }
}
