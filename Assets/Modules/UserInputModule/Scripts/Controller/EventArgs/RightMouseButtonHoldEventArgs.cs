using System;
using UnityEngine;

namespace SDRGames.Whist.UserInputModule.Controller
{
    public class RightMouseButtonSceneClickEventArgs : EventArgs
    {
        public Vector2 MousePosition { get; }

        public RightMouseButtonSceneClickEventArgs(Vector2 mousePosition)
        {
            MousePosition = mousePosition;
        }
    }
}
