using System;

using UnityEngine;

namespace SDRGames.Whist.GlobalMapModule.Models
{
    [Serializable]
    public class GlobalMapTownPinModel
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
    }
}
