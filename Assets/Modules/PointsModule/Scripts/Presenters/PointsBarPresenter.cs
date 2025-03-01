using SDRGames.Whist.PointsModule.Models;
using SDRGames.Whist.PointsModule.Views;

using UnityEngine;

namespace SDRGames.Whist.PointsModule.Presenters
{
    public class PointsBarPresenter
    {
        private Points _points;
        private PointsBarView _pointsView;

        public PointsBarPresenter(Points points, PointsBarView pointsView)
        {
            _points = points;
            _pointsView = pointsView;

            _points.CalculateValues();
            _pointsView.Initialize(points.MaxValue);

            _points.CurrentValueChanged += OnPointsCurrentValueChanged;
            _points.ReservedValueChanged += OnReservedValueChanged;
        }

        public void ReservePoints(float cost)
        {
            if(_points.CurrentValue < cost)
            {
                return;
            }
            _points.DecreaseReservedValue(cost);
        }

        public void SpendPoints(float cost)
        {
            _points.DecreaseCurrentValue(cost);
        }

        public void RestorePoints()
        {
            _points.IncreaseCurrentValue(_points.MaxValue * 0.1f);
        }

        public void ResetReservedPoints(float reverseAmount)
        {
            _points.ResetReservedValue(reverseAmount);
        }

        public string GetErrorMessage()
        {
            return _pointsView.ErrorMessage.GetLocalizedText();
        }

        private void OnPointsCurrentValueChanged(object sender, ValueChangedEventArgs e)
        {
            _pointsView.ChangeSpentFillerValue(e.NewValueInPercents);
            _pointsView.SetPointsText(e.NewValue, e.MaxValue);
        }

        private void OnReservedValueChanged(object sender, ValueChangedEventArgs e)
        {
            float currentValue = e.NewValueInPercents < 100 ? e.NewValueInPercents / 100 * e.MaxValue : e.MaxValue;
            _pointsView.ChangeReservedFillerValue(e.NewValueInPercents);
            _pointsView.SetPointsText(currentValue, e.MaxValue);
        }
    }
}
