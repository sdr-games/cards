using System;
using System.Reflection;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.UserInputModule.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "InterfaceBindings", menuName = "SDRGames/Controls/Interface Bindings")]
    public class InterfaceBindings : KeyBindings
    {
        public static string AlchemyKey { get; private set; }
        public static string BestiaryKey { get; private set; }
        public static string CardCollectionsKey { get; private set; }
        public static string JournalKey { get; private set; }
        public static string QuestLogKey { get; private set; }
        public static string TalentsKey { get; private set; }


        private void OnEnable()
        {
            if (AlchemyKey == "" || BestiaryKey == "" || CardCollectionsKey == "" || JournalKey == "" || TalentsKey == "" || QuestLogKey == "")
            {
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #endif
                throw new Exception($"Одна из кнопок не была назначена в {name}");
            }
        }

        public override string[] GetKeys()
        {
            return new string[] { AlchemyKey, BestiaryKey, CardCollectionsKey, JournalKey, TalentsKey, QuestLogKey };
        }

        public void SetKey(string keyName, string value)
        {
            PropertyInfo propertyInfo = GetType().GetProperty(keyName);
            propertyInfo.SetValue(this, Convert.ChangeType(value, propertyInfo.PropertyType), null);
        }
    }
}
