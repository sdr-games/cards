using System;

using UnityEngine;

namespace SDRGames.Islands.DiceModule.Models
{
    [Serializable]
    public class Dice
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int RollsCount { get; private set; }
        [field: SerializeField] public int SidesCount { get; private set; }
        [field: SerializeField] public int Modificator { get; private set; }

        public event EventHandler<DiceChangedEventArgs> DiceChanged;

        public Dice(string name, int rollsCount = 1, int sidesCount = 4, int modificator = 0)
        {
            Name = name;
            SidesCount = sidesCount;
            RollsCount = rollsCount;
            Modificator = modificator;
        }

        /// <summary>
        /// Roll every dice and get sum of the random values for getting total flat value.
        /// If modificator is greater than 0 result will be increased.
        /// If modificator is lesser than 0 result will be decreased.
        /// </summary>
        public int Roll()
        {
            int result = 0;
            for (int i = 0; i < RollsCount; i++)
            {
                result += UnityEngine.Random.Range(1, SidesCount);
            }

            if (Modificator != 0)
            {
                result += Modificator;
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
            for (int i = 0; i < RollsCount; i++)
            {
                result += UnityEngine.Random.Range(1, SidesCount + 1);
            }

            if (result == RollsCount)
            {
                result = 0;
            }
            else if (Modificator != 0)
            {
                result += Modificator;
            }
            return result;
        }
        public int GetCriticalValue()
        {
            return SidesCount * RollsCount + Modificator;
        }
        public string GetString(bool useDash = false)
        {
            if (RollsCount == 0 || SidesCount == 0)
            {
                return "0";
            }

            string delimeter = useDash ? "-" : " d ";
            string modificator = Modificator > 0 ? $" +{Modificator}" : "";

            return $"{RollsCount}{delimeter}{SidesCount}{modificator}";
        }
        public void Increase(Dice dice)
        {
            if (dice.RollsCount > 0)
            {
                RollsCount += dice.RollsCount;
            }
            if (dice.SidesCount > 0)
            {
                SidesCount += dice.SidesCount;
            }
            if (dice.Modificator > 0)
            {
                Modificator += dice.Modificator;
            }
            DiceChanged?.Invoke(this, new DiceChangedEventArgs(this));
        }
        public void Decrease(Dice dice)
        {
            if (dice.RollsCount > 0)
            {
                RollsCount -= dice.RollsCount;
                if(RollsCount < 0)
                {
                    RollsCount = 0;
                }
            }
            if (dice.SidesCount > 0)
            {
                SidesCount -= dice.SidesCount;
                if(SidesCount < 0)
                {
                    SidesCount = 0;
                }
            }
            if (dice.Modificator > 0)
            {
                Modificator -= dice.Modificator;
                if(Modificator < 0)
                {
                    Modificator = 0;
                }
            }
            DiceChanged?.Invoke(this, new DiceChangedEventArgs(this));
        }

        public bool Compare(Dice diceToCompare)
        {
            return RollsCount <= diceToCompare.RollsCount && SidesCount < diceToCompare.SidesCount;
        }
    }
}
