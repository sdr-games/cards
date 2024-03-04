using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Colors", menuName = "Colors", order = 52)]
public class MessageColors : ScriptableObject
{
    public static MessageColors instance;
    [SerializeField] private List<MessageColor> _colors;

    private static MessageColors GetInstance()
    {
        if(instance == null)
        {
            instance = Resources.Load("ScriptableObjectsAssets/Utility/Colors") as MessageColors;
        }
        return instance;
    }
    public static Color32 GetColor(string colorName)
    {
        MessageColor messageColor = GetInstance()._colors.Find(c => c.colorName == colorName);
        if(messageColor != null)
        {
            return messageColor.color;
        }

        return new Color32(255,255,255,255);
    }
}
