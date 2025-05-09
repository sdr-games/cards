using UnityEngine;

namespace SDRGames.Whist.AbilitiesModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BuffLogic", menuName = "SDRGames/Combat/Logics/Buff Logic")]
    public class BuffLogicScriptableObject : AbilityLogicScriptableObject
    {
        public enum BuffTypes { HealthPoints, Strength, Agility, Stamina, Intelligence, PhysicalDamage, MagicalDamage, PhysicalDamageBlock, MagicalDamageBlock, PatientDamageBlock, Sacrifice, Thorns, Converting, DebuffsBlock, ArmorPoints, BarrierPoints, Undying, UndyingPatient };
        [field: SerializeField] public BuffTypes BuffType { get; private set; }
        [field: SerializeField] public int BuffValue { get; private set; }

        private void OnEnable()
        {
            SelfUsable = true;
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
