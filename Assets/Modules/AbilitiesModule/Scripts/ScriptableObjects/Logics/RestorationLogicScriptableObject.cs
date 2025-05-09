using System;

using UnityEngine;

namespace SDRGames.Whist.AbilitiesModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "RestorationLogic", menuName = "SDRGames/Combat/Logics/Restoration Logic")]
    public class RestorationLogicScriptableObject : AbilityLogicScriptableObject
    {
        public enum RestorationTypes { Armor, Barrier, Health, Stamina, Breath, PatientHealth, Dispel, Swap };
        [field: SerializeField] public RestorationTypes RestorationType { get; private set; }
        [field: SerializeField] public int RestorationValue { get; private set; }

        private void OnEnable()
        {
            SelfUsable = true;
            InMaxPercents = true;
        }
    }
}
