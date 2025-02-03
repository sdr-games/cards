using System;

using SDRGames.Whist.RestorationModule.Models;

namespace SDRGames.Whist.RestorationModule.Managers
{
    public class PotionClickedEventArgs : EventArgs
    {
        public Potion Potion { get; private set; }

        public PotionClickedEventArgs(Potion potion)
        {
            Potion = potion;
        }
    }
}