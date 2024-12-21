using System;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.HelpersModule
{
    public static class Utility
    {
        public static void CheckFieldValueIsNotNull(this MonoBehaviour sender, string fieldName, object fieldValue)
        {
            if (fieldValue == null)
            {
                #if UNITY_EDITOR
                    Debug.LogError($"Поле {fieldName} не заполнено у {sender}!");
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }

        public static void CheckFieldValueIsNotEmpty(this MonoBehaviour sender, string fieldName, Array fieldValue)
        {
            if (fieldValue.Length <= 0)
            {
                #if UNITY_EDITOR
                    Debug.LogError($"Массив {fieldName} пустой у {sender}!");
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }

        public static void CheckFieldValueIsNotEmpty(this MonoBehaviour sender, string fieldName, List<object> fieldValue)
        {
            if (fieldValue.Count <= 0)
            {
                #if UNITY_EDITOR
                    Debug.LogError($"Список {fieldName} пустой у {sender}!");
                    EditorApplication.isPlaying = false;
                #endif
                Application.Quit();
            }
        }
    }
}
