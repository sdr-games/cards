using System;

using SDRGames.Whist.LocalizationModule.Models;

namespace SDRGames.Whist.LocalizationModule.Models
{
    public class LocalizationDataChangedEventArgs : EventArgs
    {
        public LocalizationData TextLocalization { get; private set; }

        public LocalizationDataChangedEventArgs(LocalizationData textLocalization)
        {
            TextLocalization = textLocalization;
        }
    }
}