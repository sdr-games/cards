using System;
using UnityEngine;

namespace SDRGames.SpaceTrucker.UserInputModule.Controller
{
    public class RightMouseButtonClickEventArgs : EventArgs
    {
        public Vector2 MousePosition { get; }

        public RightMouseButtonClickEventArgs(Vector2 mousePosition)
        {
            MousePosition = mousePosition;
        }
    }
}
