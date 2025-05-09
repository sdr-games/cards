using System;

namespace SDRGames.Whist.CharacterCombatModule.ScriptableObjects
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