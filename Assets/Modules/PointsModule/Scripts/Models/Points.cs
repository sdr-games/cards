using System;
using System.Collections;

using UnityEngine;

namespace SDRGames.Whist.PointsModule.Models
{
    [Serializable]
    public class Points
    {
        public string Name { get; private set; }
        [field: SerializeField] public float BaseValue { get; private set; }
        [field: SerializeField] public float Bonus { get; private set;  }
        [field: SerializeField] public float RegenerationPower { get; private set; }

        public float CurrentValue { get; private set; }
        public float CurrentValueInPercents { get; private set; }
        public float ReservedValue { get; private set; }
        public float MaxValue { get; private set; }

        public event EventHandler BonusChanged;
        public event EventHandler RegenerationPowerChanged;
        public event EventHandler<ValueChangedEventArgs> CurrentValueChanged;
        public event EventHandler<ValueChangedEventArgs> ReservedValueChanged;
        public event EventHandler MaxValueChanged;

        public void SetName(string name)
        {
            Name = name;
        }

        public void Reset()
        {
            CurrentValue = MaxValue;
            CalculateValues();
        }

        public void CalculateValues()
        {
            CurrentValueInPercents = GetValueInPercents(CurrentValue);
            CalculateMaxValue();
            CalculateCurrentValue(CurrentValueInPercents);
            ReservedValue = 0;
        }

        public void IncreaseBonus(float bonus)
        {
            Bonus += bonus;
            CalculateValues();
            BonusChanged?.Invoke(this, new EventArgs());
        }

        public void DecreaseBonus(float bonus)
        {
            Bonus -= bonus;
            CalculateValues();
            BonusChanged?.Invoke(this, new EventArgs());
        }

        public void IncreaseRegenerationPower(float regenerationPower)
        {
            RegenerationPower += regenerationPower;
            CalculateValues();
            RegenerationPowerChanged?.Invoke(this, new EventArgs());
        }

        public void DecreaseRegenerationPower(float regenerationPower)
        {
            RegenerationPower -= regenerationPower;
            CalculateValues();
            RegenerationPowerChanged?.Invoke(this, new EventArgs());
        }

        public void IncreaseCurrentValue(float value)
        {
            CurrentValue += value;
            if(CurrentValue > MaxValue)
            {
                CurrentValue = MaxValue;
            }
            ResetReservedValue(ReservedValue);
            CurrentValueInPercents = GetValueInPercents(CurrentValue);
            CurrentValueChanged?.Invoke(this, new ValueChangedEventArgs(CurrentValue, CurrentValueInPercents, MaxValue));
        }

        public void DecreaseCurrentValue(float cost)
        {
            if(cost > CurrentValue)
            {
                cost = CurrentValue;
            }
            CurrentValue -= cost;
            ResetReservedValue(ReservedValue);
            CurrentValueInPercents = GetValueInPercents(CurrentValue);
            CurrentValueChanged?.Invoke(this, new ValueChangedEventArgs(CurrentValue, CurrentValueInPercents, MaxValue));
        }

        public void DecreaseReservedValue(float value)
        {
            ReservedValue += value;
            ReservedValueChanged?.Invoke(this, new ValueChangedEventArgs(ReservedValue, GetValueInPercents(CurrentValue - ReservedValue), MaxValue));
        }

        public void ResetReservedValue(float reverseAmount)
        {
            ReservedValue -= reverseAmount;
            ReservedValueChanged?.Invoke(this, new ValueChangedEventArgs(ReservedValue, GetValueInPercents(CurrentValue - ReservedValue), MaxValue));
        }

        public IEnumerator Regenerate(float regenerationPower = 0, float regenerationSpeed = 0, float length = 0)
        {
            if (regenerationPower == 0)
            {
                regenerationPower = RegenerationPower;
            }

            while (length >= 0 && CurrentValue < MaxValue)
            {
                yield return new WaitForSeconds(regenerationSpeed);
                if (length > 0)
                {
                    length -= regenerationSpeed;
                }
                IncreaseCurrentValue(regenerationPower);
            }
        }

        public void SetBaseValue(float baseValue)
        {
            BaseValue = baseValue;
            CalculateValues();
        }

        private float GetValueInPercents(float value)
        {
            if(MaxValue == 0)
            {
                return 100;
            }
            return value * 100 / MaxValue;
        }

        private void CalculateMaxValue()
        {
            MaxValue = BaseValue + Bonus;
            MaxValueChanged?.Invoke(this, new EventArgs());
        }

        private void CalculateCurrentValue(float currentValueInPercents)
        {
            CurrentValue = MaxValue * currentValueInPercents / 100;
            CurrentValueInPercents = GetValueInPercents(CurrentValue);
            CurrentValueChanged?.Invoke(this, new ValueChangedEventArgs(CurrentValue, CurrentValueInPercents, MaxValue));
        }
    }
}
