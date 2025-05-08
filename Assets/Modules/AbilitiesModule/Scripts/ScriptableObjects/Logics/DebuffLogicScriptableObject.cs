using UnityEngine;

namespace SDRGames.Whist.AbilitiesModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DebuffLogic", menuName = "SDRGames/Combat/Logics/Debuff Logic")]
    public class DebuffLogicScriptableObject : AbilityLogicScriptableObject
    {
        public enum DebuffTypes { Strength, Agility, Stamina, Intelligence, PhysicalDamage, MagicalDamage, PhysicalDamageBlock, MagicalDamageBlock, PatientDamageBlock, Insanity, Advantage };
        [field: SerializeField] public DebuffTypes DebuffType { get; private set; }
        [field: SerializeField] public int DebuffValue { get; private set; }

        private void OnEnable()
        {
            SelfUsable = false;
            InMaxPercents = true;
        }

        private void OnValidate()
        {
            if(RoundsCount < 1)
            {
                RoundsCount = 1;
            }
        }
    }
}
