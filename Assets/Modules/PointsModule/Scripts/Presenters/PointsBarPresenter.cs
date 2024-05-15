using SDRGames.Whist.PointsModule.Models;
using SDRGames.Whist.PointsModule.Views;

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
            _pointsView.ChangeSpentFillerValue(e.CurrentValueInPercents);
            _pointsView.SetPointsText(e.CurrentValue, e.MaxValue);
        }

        private void OnReservedValueChanged(object sender, ValueChangedEventArgs e)
        {
            float currentValue = e.CurrentValueInPercents / 100 * e.MaxValue;
            _pointsView.ChangeReservedFillerValue(e.CurrentValueInPercents);
            _pointsView.SetPointsText(currentValue, e.MaxValue);
        }
    }
}
