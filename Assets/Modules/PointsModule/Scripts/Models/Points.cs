using System;
using System.Collections;

using SDRGames.Whist.HelpersModule;

using UnityEngine;

namespace SDRGames.Whist.PointsModule.Models
{
    [Serializable]
    public class Points
    {
        public string Name { get; private set; }
        [field: SerializeField] public float BaseValue { get; private set; }
        [field: SerializeField][field: ReadOnly] public float PermanentBonus { get; private set; }
        [field: SerializeField][field: ReadOnly] public float TemporaryBonus { get; private set; }
        [field: SerializeField] public float RestorationPower { get; private set; }
        [field: SerializeField][field: ReadOnly] public float MaxValue { get; private set; }

        public float CurrentValue { get; private set; }
        public float CurrentValueInPercents { get; private set; }
        public float ReservedValue { get; private set; }

        public event EventHandler PermanentBonusChanged;
        public event EventHandler TemporaryBonusChanged;
        public event EventHandler RegenerationPowerChanged;
        public event EventHandler<ValueChangedEventArgs> CurrentValueChanged;
        public event EventHandler<ValueChangedEventArgs> ReservedValueChanged;
        public event EventHandler<ValueChangedEventArgs> MaxValueChanged;

        public Points(Points points)
        {
            Name = points.Name;
            BaseValue = points.BaseValue;
            PermanentBonus = points.PermanentBonus;
            TemporaryBonus = points.TemporaryBonus;
            RestorationPower = points.RestorationPower;
            MaxValue = points.MaxValue;
            Reset();
        }

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

        public void SetPermanentBonus(float bonus)
        {
            PermanentBonus = bonus;
            CalculateValues();
            PermanentBonusChanged?.Invoke(this, EventArgs.Empty);
        }

        public void IncreasePermanentBonus(float bonus)
        {
            PermanentBonus += bonus;
            CalculateValues();
            PermanentBonusChanged?.Invoke(this, EventArgs.Empty);
        }

        public void DecreasePermanentBonus(float bonus)
        {
            PermanentBonus -= bonus;
            CalculateValues();
            PermanentBonusChanged?.Invoke(this, EventArgs.Empty);
        }

        public void IncreaseTemporaryBonus(float bonus)
        {
            TemporaryBonus += bonus;
            CalculateValues();
            TemporaryBonusChanged?.Invoke(this, EventArgs.Empty);
        }

        public void DecreaseTemporaryBonus(float bonus)
        {
            TemporaryBonus -= bonus;
            CalculateValues();
            TemporaryBonusChanged?.Invoke(this, EventArgs.Empty);
        }

        public void SetRestorationPower(float restorationPower)
        {
            RestorationPower = restorationPower;
            RegenerationPowerChanged?.Invoke(this, EventArgs.Empty);
        }

        public void IncreaseRestorationPower(float restorationPower)
        {
            RestorationPower += restorationPower;
            RegenerationPowerChanged?.Invoke(this, EventArgs.Empty);
        }

        public void DecreaseRestorationPower(float restorationPower)
        {
            RestorationPower -= restorationPower;
            RegenerationPowerChanged?.Invoke(this, EventArgs.Empty);
        }

        public void IncreaseCurrentValue(float value, bool isCritical = false)
        {
            float originalValue = CurrentValue;
            CurrentValue += value;
            if(CurrentValue > MaxValue)
            {
                CurrentValue = MaxValue;
            }
            ResetReservedValue(ReservedValue);
            CurrentValueInPercents = GetValueInPercents(CurrentValue);
            CurrentValueChanged?.Invoke(this, new ValueChangedEventArgs(originalValue, CurrentValue, CurrentValueInPercents, MaxValue, isCritical));
        }

        public void DecreaseCurrentValue(float value, bool isCritical = false)
        {
            float originalValue = CurrentValue;
            if (value > CurrentValue)
            {
                value = CurrentValue;
            }
            CurrentValue -= value;
            ResetReservedValue(ReservedValue);
            CurrentValueInPercents = GetValueInPercents(CurrentValue);
            CurrentValueChanged?.Invoke(this, new ValueChangedEventArgs(originalValue, CurrentValue, CurrentValueInPercents, MaxValue, isCritical));
        }

        public void DecreaseReservedValue(float value)
        {
            float originalValue = CurrentValue;
            ReservedValue += value;
            ReservedValueChanged?.Invoke(this, new ValueChangedEventArgs(originalValue, ReservedValue, GetValueInPercents(CurrentValue - ReservedValue), MaxValue, false));
        }

        public void ResetReservedValue(float reverseAmount)
        {
            float originalValue = CurrentValue;
            ReservedValue -= reverseAmount;
            ReservedValueChanged?.Invoke(this, new ValueChangedEventArgs(originalValue, ReservedValue, GetValueInPercents(CurrentValue - ReservedValue), MaxValue, false));
        }

        public IEnumerator Regenerate(float regenerationPower = 0, float regenerationSpeed = 0, float length = 0)
        {
            if (regenerationPower == 0)
            {
                regenerationPower = MaxValue * (RestorationPower / 100);
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
            float originalValue = CurrentValue;
            CalculateCurrentValue(GetValueInPercents(CurrentValue));
            MaxValue = BaseValue + PermanentBonus + TemporaryBonus;
            MaxValueChanged?.Invoke(this, new ValueChangedEventArgs(originalValue, CurrentValue, CurrentValueInPercents, MaxValue, false));
        }

        private void CalculateCurrentValue(float currentValueInPercents)
        {
            float originalValue = CurrentValue;
            CurrentValue = MaxValue * currentValueInPercents / 100;
            CurrentValueInPercents = GetValueInPercents(CurrentValue);
            CurrentValueChanged?.Invoke(this, new ValueChangedEventArgs(originalValue, CurrentValue, CurrentValueInPercents, MaxValue, false));
        }
    }
}
