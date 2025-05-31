using System;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.ActiveBlockModule.Models
{
    [Serializable]
    public class ActiveBlockSideModelC
    {
        [field: SerializeField] public Image SideImage { get; private set; }
        [field: SerializeField] public KeyCode CorrectKey { get; private set; }
    }
}
