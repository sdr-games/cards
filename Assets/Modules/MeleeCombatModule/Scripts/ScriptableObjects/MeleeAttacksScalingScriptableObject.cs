using SDRGames.Whist.MeleeCombatModule.Models;

using UnityEngine;

namespace SDRGames.Whist.MeleeCombatModule.ScriptableObjects
{
    //[CreateAssetMenu(fileName = "MeleeAttacksScaling", menuName = "SDRGames/Combat/Melee/Scaling settings")]
    public class MeleeAttacksScalingScriptableObject : ScriptableObject
    {
        [SerializeField] private MeleeAttacksScaling _meleeAttacksScaling;

        public void Initialize()
        {
            _meleeAttacksScaling.UpdateStaticFields();
        }

        private void OnValidate()
        {
            _meleeAttacksScaling.UpdateStaticFields();
        }
    }
}
