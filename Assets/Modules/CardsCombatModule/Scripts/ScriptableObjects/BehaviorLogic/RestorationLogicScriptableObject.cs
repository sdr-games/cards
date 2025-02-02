using System;

using UnityEngine;

namespace SDRGames.Whist.CardsCombatModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "RestorationLogic", menuName = "SDRGames/Combat/Cards/Logics/Restoration Logic")]
    public class RestorationLogicScriptableObject : CardLogicScriptableObject
    {
        public enum RestorationTypes { Armor, Barrier, Health, Stamina, Breath };
        [field: SerializeField] public RestorationTypes RestorationType { get; private set; }
        [field: SerializeField] public int RestorationValue { get; private set; }

        private void OnEnable()
        {
            SelfUsable = true;
        }
    }
}
