using System;

using UnityEngine;
using UnityEngine.UI;

namespace SDRGames.Whist.ActiveBlockModule.Models
{
    [Serializable]
    public class ActiveBlockSideModel
    {
        [field: SerializeField] public Image SideImage { get; private set; }
        [field: SerializeField] public KeyCode CorrectKey { get; private set; }
        public Vector2 DefaultPosition { get; private set; }

        public void SetDefaultPosition(Vector2 defaultPosition)
        {
            DefaultPosition = defaultPosition;
        }
    }
}
