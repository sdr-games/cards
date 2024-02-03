using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Phrase
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public string Text { get; private set; }
    [field: SerializeField] public Sprite Portrait { get; private set; }
}
