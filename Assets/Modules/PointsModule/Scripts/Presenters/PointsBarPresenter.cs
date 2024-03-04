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
        }

        private void OnPointsCurrentValueChanged(object sender, CurrentValueChangedEventArgs e)
        {
            _pointsView.ChangeFillerValue(e.CurrentValueInPercents);
            _pointsView.SetPointsText(e.CurrentValue);
        }
    }
}
