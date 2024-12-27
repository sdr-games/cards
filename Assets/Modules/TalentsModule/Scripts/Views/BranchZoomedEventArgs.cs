using System;

namespace SDRGames.Whist.TalentsModule.Views
{
    public class BranchZoomedEventArgs : EventArgs
    {
        public float Angle { get; private set; }
        public float Time { get; private set; }

        public BranchZoomedEventArgs(float angle, float time)
        {
            Angle = angle;
            Time = time;
        }
    }
}