using System;

using SDRGames.Whist.RestorationModule.ScriptableObjects;

namespace SDRGames.Whist.RestorationModule.Managers
{
    public class PotionClickedEventArgs : EventArgs
    {
        public PotionScriptableObject PotionScriptableObject { get; private set; }

        public PotionClickedEventArgs(PotionScriptableObject potionScriptableObject)
        {
            PotionScriptableObject = potionScriptableObject;
        }
    }
}