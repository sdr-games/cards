using SDRGames.Whist.PointsModule.Models;
using SDRGames.Whist.PointsModule.Views;

namespace SDRGames.Whist.PointsModule.Presenters
{
    public class PointsTextPresenter
    {
        private Points _points;
        private PointsTextView _pointsView;

        public PointsTextPresenter(Points points, PointsTextView pointsView)
        {
            _points = points;
            _pointsView = pointsView;

            _points.CalculateValues();
            _pointsView.Initialize(points.Name, points.MaxValue);

            _points.CurrentValueChanged += OnPointsCurrentValueChanged;
        }

        private void OnPointsCurrentValueChanged(object sender, ValueChangedEventArgs e)
        {
            _pointsView.SetPointsText(e.CurrentValue);
        }
    }
}
