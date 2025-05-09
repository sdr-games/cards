using SDRGames.Whist.CharacterCombatModule.Models;

using UnityEngine;

namespace SDRGames.Whist.CharacterCombatModule.ScriptableObjects
{
    //[CreateAssetMenu(fileName = "CharacterParametersScaling", menuName = "SDRGames/Characters/Scaling settings")]
    public class CharacterParametersScalingScriptableObject : ScriptableObject
    {
        [SerializeField] private CharacterParametersScaling _characterParametersScaling;

        public void Initialize()
        {
            _characterParametersScaling.UpdateStaticFields();
        }

        private void OnValidate()
        {
            _characterParametersScaling.UpdateStaticFields();
        }
    }
}
