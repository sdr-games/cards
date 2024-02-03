using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PinTip
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }

    public PinTip(string name, string description, Sprite sprite)
    {
        Name = name;
        Description = description;
        Sprite = sprite;
    }
}
