using System;

namespace SDRGames.Islands.DiceModule
{
    [Serializable]
    public class Dice
    {
        //TODO: make private
        public int _rollsCount = 1;
        public int _sidesCount = 4;
        public int _modificator;

        public Dice(int rollsCount = 1, int sidesCount = 4, int modificator = 0)
        {
            this._sidesCount = sidesCount;
            this._rollsCount = rollsCount;
            this._modificator = modificator;
        }

        /// <summary>
        /// Roll every dice and get sum of the random values for getting total flat value.
        /// If modificator is greater than 0 result will be increased.
        /// If modificator is lesser than 0 result will be decreased.
        /// </summary>
        public int Roll()
        {
            int result = 0;
            for (int i = 0; i < _rollsCount; i++)
            {
                result += UnityEngine.Random.Range(1, _sidesCount);
            }

            if (_modificator != 0)
            {
                result += _modificator;
            }
            return result;
        }

        /// <summary>
        /// Roll every dice and get sum of the random values for getting chance value.
        /// If modificator is greater than 0 result will be increased.
        /// If modificator is lesser than 0 result will be decreased.
        /// </summary>
        public int CheckRoll()
        {
            int result = 0;
            for (int i = 0; i < _rollsCount; i++)
            {
                result += UnityEngine.Random.Range(1, _sidesCount + 1);
            }

            if (result == _rollsCount)
            {
                result = 0;
            }
            else if (_modificator != 0)
            {
                result += _modificator;
            }
            return result;
        }
        public int GetCriticalValue()
        {
            return _sidesCount * _rollsCount + _modificator;
        }
        public string GetString()
        {
            if (_rollsCount == 0 || _sidesCount == 0)
            {
                return "0";
            }

            string result = _rollsCount + "d" + _sidesCount;
            if (_modificator > 0)
            {
                result += " +" + _modificator;
            }
            return result;
        }
        public void Increase(Dice dice)
        {
            _rollsCount += dice._rollsCount;
            _sidesCount += dice._sidesCount;
            _modificator += dice._modificator;
        }
        public void Decrease(Dice dice)
        {
            _rollsCount -= dice._rollsCount;
            _sidesCount -= dice._sidesCount;
            _modificator -= dice._modificator;
        }

        public bool Compare(Dice diceToCompare)
        {
            return _rollsCount <= diceToCompare._rollsCount && _sidesCount < diceToCompare._sidesCount;
        }
    }
}
