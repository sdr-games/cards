using System;
using UnityEditor;

using System.Reflection;

using UnityEngine;

namespace SDRGames.Whist.UserInputModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "CombatBindings", menuName = "SDRGames/Controls/Combat Bindings")]
    public class CombatBindings : KeyBindings
    {
        public static string QuickStrikeKey { get; private set; }
        public static string FirstCardKey { get; private set; }
        public static string MediumStrikeKey { get; private set; }
        public static string SecondCardKey { get; private set; }
        public static string HeavyStrikeKey { get; private set; }
        public static string ThirdCardKey { get; private set; }
        public static string FourthCardKey { get; private set; }
        public static string EmptySlotKey { get; private set; }
        public static string DeleteLastSlotKey { get; private set; }


        private void OnEnable()
        {
            if (QuickStrikeKey == ""
                || FirstCardKey == "" 
                || MediumStrikeKey == "" 
                || SecondCardKey == ""
                || HeavyStrikeKey == ""
                || ThirdCardKey == "" 
                || FourthCardKey == "" 
                || DeleteLastSlotKey == "" 
                || EmptySlotKey == ""
            )
            {
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                throw new Exception($"Одна из кнопок не была назначена в {name}");
            }
        }

        public override string[] GetKeys()
        {
            return new string[] { QuickStrikeKey, FirstCardKey, MediumStrikeKey, SecondCardKey, HeavyStrikeKey, ThirdCardKey, FourthCardKey, DeleteLastSlotKey, EmptySlotKey };
        }

        public void SetKey(string keyName, string value)
        {
            PropertyInfo propertyInfo = GetType().GetProperty(keyName);
            propertyInfo.SetValue(this, Convert.ChangeType(value, propertyInfo.PropertyType), null);
        }
    }
}
