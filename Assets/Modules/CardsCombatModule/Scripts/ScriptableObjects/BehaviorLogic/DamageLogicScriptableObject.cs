using SDRGames.Whist.CardsCombatModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesModule.ScriptableObjects
{
    [CreateAssetMenu(fileName = "DamageLogic", menuName = "SDRGames/Combat/Cards/Logics/Damage Logic")]
    public class DamageLogicScriptableObject : CardLogicScriptableObject
    {
        public enum DamageTypes { Physical, Magical, True };
        [field: SerializeField] public DamageTypes DamageType { get; private set; }
        [field: SerializeField] public int DamageValue { get; private set; }  
        [field: SerializeField] public bool InPercents { get; private set; }

        private void OnEnable()
        {
            SelfUsable = false;
        }
    }
}
