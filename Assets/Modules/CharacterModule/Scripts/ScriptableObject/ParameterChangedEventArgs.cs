using System;

namespace SDRGames.Whist.CharacterModule.ScriptableObjects
{
    public class ParameterChangedEventArgs : EventArgs
    {
        public float ParameterValue { get; private set; }

        public ParameterChangedEventArgs(int parameterValue)
        {
            ParameterValue = parameterValue;
        }

        public ParameterChangedEventArgs(float parameterValue)
        {
            ParameterValue = parameterValue;
        }
    }
}