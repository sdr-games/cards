using System;

namespace SDRGames.Whist.PointsModule.Models
{
    public class ValueChangedEventArgs : EventArgs
    {
        public float OriginalValue { get; private set; }
        public float NewValue { get; private set; }
        public float NewValueInPercents { get; private set; }
        public float Difference => NewValue - OriginalValue;
        public float MaxValue { get; private set; }
        public bool IsCritical { get; private set; }

        public ValueChangedEventArgs(float originalValue, float newValue, float newValueInPercents, float maxValue, bool isCritical)
        {
            OriginalValue = originalValue;
            NewValue = newValue;
            NewValueInPercents = newValueInPercents;
            MaxValue = maxValue;
            IsCritical = isCritical;
        }
    }
}
