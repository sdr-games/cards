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
                //#if UNITY_EDITOR
                    Debug.LogError($"Поле {fieldName} не заполнено у {sender}!");
                //    EditorApplication.isPlaying = false;
                //#endif
                //Application.Quit();
            }
        }

        public static void CheckFieldValueIsNotNull(this ScriptableObject sender, string fieldName, object fieldValue)
        {
            if (fieldValue == null)
            {
                //#if UNITY_EDITOR
                    Debug.LogError($"Поле {fieldName} не заполнено у {sender}!");
                //    EditorApplication.isPlaying = false;
                //#endif
                //Application.Quit();
            }
        }
    }
}
