using System;

using UnityEngine;

namespace SDRGames.Whist.UserInputModule.Controller
{
    public class RightMouseButtonUIClickEventArgs : EventArgs
    {
        public GameObject GameObject { get; }
        public Vector2 MousePosition { get; }

        public RightMouseButtonUIClickEventArgs(GameObject gameObject, Vector2 mousePosition)
        {
            GameObject = gameObject;
            MousePosition = mousePosition;
        }
    }
}