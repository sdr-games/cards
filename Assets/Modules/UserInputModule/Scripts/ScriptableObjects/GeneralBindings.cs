using System;

using UnityEngine;
using UnityEngine.InputSystem;

namespace SDRGames.Whist.UserInputModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "GeneralBindings", menuName = "SDRGames/Controls/General Bindings")]
    public class GeneralBindings : KeyBindings
    {
        public static string MenuKey { get; private set; }
        public static string QuickLoadKey { get; private set; }
        public static string QuickSaveKey { get; private set; }

        private void OnEnable()
        {
            MenuKey = Key.Escape.ToString();
            QuickLoadKey = Key.F10.ToString();
            QuickSaveKey = Key.F9.ToString();
        }

        public override string[] GetKeys()
        {
            return new string[] { MenuKey, QuickLoadKey, QuickSaveKey };
        }
    }
}
