using System;
using UnityEngine;

namespace SDRGames.Whist.UserInputModule.Controller
{
    public class LeftMouseButtonSceneClickEventArgs : EventArgs
    {
        public GameObject GameObject { get; }
        public Vector3 RaycastHitPoint { get; }

        public LeftMouseButtonSceneClickEventArgs(GameObject gameObject, Vector3 raycastHitPoint)
        {
            GameObject = gameObject;
            RaycastHitPoint = raycastHitPoint;
        }
    }
}
