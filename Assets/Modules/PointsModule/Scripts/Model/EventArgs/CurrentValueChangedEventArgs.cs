using System;

using UnityEngine;

namespace SDRGames.Islands.PointsModule.Model
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
