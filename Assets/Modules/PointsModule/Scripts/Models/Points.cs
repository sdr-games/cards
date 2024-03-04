using System;
using System.Collections;

using UnityEngine;

namespace SDRGames.Whist.PointsModule.Models
{
    [Serializable]
    public class Points
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public float BaseValue { get; private set; }
        [field: SerializeField] public float Bonus { get; private set;  }
        [field: SerializeField] public RelatedBonus[] RelatedBonuses { get; private set; }
        [field: SerializeField] public float RegenerationSpeed { get; private set; }
        [field: SerializeField] public float RegenerationPower { get; private set; }

        public float CurrentValue { get; private set; }
        public float MaxValue { get; private set; }

        public event EventHandler BonusChanged;
        public event EventHandler RegenerationPowerChanged;
        public event EventHandler<CurrentValueChangedEventArgs> CurrentValueChanged;
        public event EventHandler MaxValueChanged;

        public Points(string name, float baseValue, float bonus, RelatedBonus[] relatedBonuses, float regenerationSpeed, float regenerationPower)
        {
            Name = name;
            BaseValue = baseValue;
            Bonus = bonus;
            RelatedBonuses = relatedBonuses;
            RegenerationSpeed = regenerationSpeed;
            RegenerationPower = regenerationPower;
        }

        public void CalculateValues()
        {
            float currentValueInPercents = GetCurrentValueInPercents();
            CalculateMaxValue();
            CalculateCurrentValue(currentValueInPercents);
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
            CurrentValueChanged?.Invoke(this, new CurrentValueChangedEventArgs(CurrentValue, GetCurrentValueInPercents()));
        }

        public void DecreaseCurrentValue(float value)
        {
            CurrentValue -= value;
            CurrentValueChanged?.Invoke(this, new CurrentValueChangedEventArgs(CurrentValue, GetCurrentValueInPercents()));
        }

        public IEnumerator Regenerate(float regenerationPower = 0, float regenerationSpeed = 0, float length = 0)
        {
            if (regenerationPower == 0)
            {
                regenerationPower = RegenerationPower;
            }

            if (regenerationSpeed == 0)
            {
                regenerationSpeed = RegenerationSpeed;
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

        private float GetCurrentValueInPercents()
        {
            if(MaxValue == 0)
            {
                return 100;
            }
            return CurrentValue * 100 / MaxValue;
        }

        private void CalculateMaxValue()
        {
            MaxValue = BaseValue + Bonus + GetTotalRelatedBonus();
            MaxValueChanged?.Invoke(this, new EventArgs());
        }

        private float GetTotalRelatedBonus()
        {
            float totalRelatedBonus = 0;
            foreach(RelatedBonus relatedBonus in RelatedBonuses)
            {
                totalRelatedBonus += relatedBonus.GetBonus();
            }
            return totalRelatedBonus;
        }

        private void CalculateCurrentValue(float currentValueInPercents)
        {
            CurrentValue = MaxValue * currentValueInPercents / 100;
            CurrentValueChanged?.Invoke(this, new CurrentValueChangedEventArgs(CurrentValue, GetCurrentValueInPercents()));
        }
    }
}
