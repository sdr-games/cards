using UnityEngine;

namespace SDRGames.Whist.AbilitiesModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DamageLogic", menuName = "SDRGames/Combat/Logics/Damage Logic")]
    public class DamageLogicScriptableObject : AbilityLogicScriptableObject
    {
        public enum DamageTypes { Physical, Magical, True, TruePatient };
        [field: SerializeField] public DamageTypes DamageType { get; private set; }
        [field: SerializeField] public int DamageValue { get; private set; }

        private void OnEnable()
        {
            SelfUsable = false;
        }
    }
}
