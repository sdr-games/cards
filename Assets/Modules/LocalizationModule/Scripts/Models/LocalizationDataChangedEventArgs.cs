using System;

using SDRGames.Whist.LocalizationModule.Models;

namespace SDRGames.Whist.LocalizationModule.Models
{
    public class LocalizationDataChangedEventArgs : EventArgs
    {
        public LocalizationData DescriptionLocalization { get; private set; }

        public LocalizationDataChangedEventArgs(LocalizationData descriptionLocalization)
        {
            DescriptionLocalization = descriptionLocalization;
        }
    }
}