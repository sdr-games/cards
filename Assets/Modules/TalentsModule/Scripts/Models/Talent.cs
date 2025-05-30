using System;

namespace SDRGames.Whist.TalentsModule.Models
{
    public class Talent
    {
        public int CurrentPoints { get; private set; }
        public int TotalCost { get; private set; }
        public string Description { get; private set; }

        public event EventHandler<CurrentPointsChangedEventArgs> CurrentPointsChanged;

        public Talent(int totalCost, string description)
        {
            CurrentPoints = 0;
            TotalCost = totalCost;
            Description = description;
        }

        public void ResetCurrentPoints()
        {
            CurrentPoints = 0;
            CurrentPointsChanged?.Invoke(this, new CurrentPointsChangedEventArgs(CurrentPoints));
        }

        public void IncreaseCurrentPoints()
        {
            if(CurrentPoints < TotalCost)
            {
                CurrentPoints++;
                CurrentPointsChanged?.Invoke(this, new CurrentPointsChangedEventArgs(CurrentPoints));
            }
        }

        public void DecreaseCurrentPoints()
        {
            if(CurrentPoints > 0)
            {
                CurrentPoints--;
                CurrentPointsChanged?.Invoke(this, new CurrentPointsChangedEventArgs(CurrentPoints));
            }
        }
    }
}
