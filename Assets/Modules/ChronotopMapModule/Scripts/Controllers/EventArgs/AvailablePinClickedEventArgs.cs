using System;

using SDRGames.Whist.BezierModule.Views;
using SDRGames.Whist.DialogueModule.ScriptableObjects;

using UnityEngine;

namespace SDRGames.Whist.ChronotopMapModule.Controllers
{
    public class AvailablePinClickedEventArgs : EventArgs
    {
        public BezierView BezierView { get; private set; }

        public DialogueContainerScriptableObject DialogueContainerScriptableObject { get; private set; }

        public AvailablePinClickedEventArgs(BezierView bezierView, DialogueContainerScriptableObject dialogueContainerScriptableObject = null)
        {
            BezierView = bezierView;
            DialogueContainerScriptableObject = dialogueContainerScriptableObject;
        }
    }
}
