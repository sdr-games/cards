using System;

using UnityEngine;

namespace SDRGames.Whist.ChronotopMapModule.Models
{
    [Serializable]
    public class ChronotopMapTownPinModel
    {
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
    }
}
