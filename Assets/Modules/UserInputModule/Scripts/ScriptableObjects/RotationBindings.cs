using System;

using UnityEditor;

using UnityEngine;
using UnityEngine.InputSystem;

namespace SDRGames.Whist.UserInputModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "PlayerRotationBindings", menuName = "SDRGames/Movement Controls/Rotation Bindings")]
    public class RotationBindings : KeyBindings
    {
        [field: SerializeField] public Key RotateLeftKey { get; private set; }
        [field: SerializeField] public Key RotateRightKey { get; private set; }
        [field: SerializeField] public Key RotateToCenterKey { get; private set; }

        private void OnEnable()
        {
            if (RotateLeftKey == Key.None || RotateRightKey == Key.None || RotateToCenterKey == Key.None)
            {
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                throw new Exception($"Одна из кнопок не была назначена в {name}");
            }
        }

        public override Key[] GetKeys()
        {
            return new Key[] { RotateLeftKey, RotateRightKey, RotateToCenterKey };
        }
    }
}
