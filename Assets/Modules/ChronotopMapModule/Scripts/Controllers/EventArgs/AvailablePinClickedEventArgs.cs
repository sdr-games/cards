using System;

using SDRGames.Whist.BezierModule.Views;

using UnityEngine;

namespace SDRGames.Whist.ChronotopMapModule.Controllers
{
    public class AvailablePinClickedEventArgs : EventArgs
    {
        public BezierView BezierView { get; private set; }

        public AvailablePinClickedEventArgs(BezierView bezierView)
        {
            BezierView = bezierView;
        }
    }
}
