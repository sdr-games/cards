using System;

using UnityEditor;

using UnityEngine;
using UnityEngine.InputSystem;

namespace SDRGames.Whist.UserInputModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "PlayerMovementBindings", menuName = "SDRGames/Controls/Movement Bindings")]
    public class MovementBindings : KeyBindings
    {
        [field: SerializeField] public Key AccelerateKey { get; private set; }
        [field: SerializeField] public Key BrakeKey { get; private set; }
        [field: SerializeField] public Key SprintKey { get; private set; }

        private void OnEnable()
        {
            if(AccelerateKey == Key.None || BrakeKey == Key.None || SprintKey == Key.None)
            {
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                throw new Exception($"Одна из кнопок не была назначена в {name}");
            }
        }

        public override string[] GetKeys()
        {
            return new string[0];
            //return new Key[] { AccelerateKey, BrakeKey, SprintKey };
        }
    }
}
