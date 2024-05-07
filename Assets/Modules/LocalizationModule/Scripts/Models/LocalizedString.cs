using System;

using UnityEngine;

namespace SDRGames.Whist.LocalizationModule.Models
{
    [Serializable]
    public class LocalizedString
    {
        [field: SerializeField] public UnityEngine.Localization.LocalizedString Entity { get; private set; }

        public LocalizedString(UnityEngine.Localization.LocalizedString entity)
        {
            Entity = entity;
        }

        public string GetLocalizedText()
        {
            return Entity.GetLocalizedString();
        }
    }
}
