using System;

namespace SDRGames.Whist.SceneManagementModule.Initializers
{
    public class PartInitializedEventArgs : EventArgs
    {
        public float CurrentPercent { get; private set; }

        public PartInitializedEventArgs(float currentPercent)
        {
            CurrentPercent = currentPercent;
        }
    }
}