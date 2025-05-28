using System;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.ActiveBlockModule.Models
{
    [Serializable]
    public class ActiveBlockSideModel
    {
        [field: SerializeField] public Image SideImage { get; private set; }
        [field: SerializeField] public Vector2 StartOffsetMax { get; private set; }
        [field: SerializeField] public Vector2 StartOffsetMin { get; private set; }
    }
}
