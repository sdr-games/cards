using System;

using UnityEngine;

namespace SDRGames.Whist.PointsModule.Models
{
    public class CurrentValueChangedEventArgs : EventArgs
    {
        [field: SerializeField] public float CurrentValue { get; }
        [field: SerializeField] public float CurrentValueInPercents { get; }

        public CurrentValueChangedEventArgs(float currentValue, float currentValueInPercents)
        {
            CurrentValue = currentValue;
            CurrentValueInPercents = currentValueInPercents;
        }
    }
}
