using System;
using UnityEngine;

namespace SDRGames.Whist.UserInputModule.Controller
{
    public class LeftMouseButtonUIClickEventArgs : EventArgs
    {
        public GameObject GameObject { get; }
        public Vector2 MousePosition { get; }

        public LeftMouseButtonUIClickEventArgs(GameObject gameObject, Vector2 mousePosition)
        {
            GameObject = gameObject;
            MousePosition = mousePosition;
        }
    }
}
