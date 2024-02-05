using System;
using UnityEngine;

namespace SDRGames.SpaceTrucker.UserInputModule.Controller
{
    public class MiddleMouseButtonScrollEventArgs : EventArgs
    {
        public Vector2 ScrollPosition { get; }

        public MiddleMouseButtonScrollEventArgs(Vector2 scrollPosition)
        {
            ScrollPosition = scrollPosition;
        }
    }
}
