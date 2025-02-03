using UnityEngine;

namespace SDRGames.Whist.AbilitiesModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BuffLogic", menuName = "SDRGames/Combat/Logics/Buff Logic")]
    public class BuffLogicScriptableObject : AbilityLogicScriptableObject
    {
        public enum BuffTypes { HealthPoints, Strength, Agility, Stamina, Intelligence, PhysicalDamage, MagicDamage };
        [field: SerializeField] public BuffTypes BuffType { get; private set; }
        [field: SerializeField] public int BuffValue { get; private set; }
        [field: SerializeField] public bool InPercents { get; private set; }

        private void OnEnable()
        {
            SelfUsable = true;
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
