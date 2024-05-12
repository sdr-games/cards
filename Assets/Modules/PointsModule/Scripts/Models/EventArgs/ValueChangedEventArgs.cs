using System;

using UnityEngine;

namespace SDRGames.Whist.PointsModule.Models
{
    public class ValueChangedEventArgs : EventArgs
    {
        [field: SerializeField] public float CurrentValue { get; }
        [field: SerializeField] public float CurrentValueInPercents { get; }
        [field: SerializeField] public float MaxValue { get; }

        public ValueChangedEventArgs(float currentValue, float currentValueInPercents, float maxValue)
        {
            CurrentValue = currentValue;
            CurrentValueInPercents = currentValueInPercents;
            MaxValue = maxValue;
        }
    }
}
